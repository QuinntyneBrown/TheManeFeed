// Independent scraper verification script
// Uses Playwright to visit each scraper URL and extract visible article titles
// Results are saved to tests/verification-results/

import { chromium } from 'playwright';
import { readFile, mkdir, writeFile } from 'fs/promises';
import { existsSync } from 'fs';
import { join, dirname } from 'path';
import { fileURLToPath } from 'url';

const __dirname = dirname(fileURLToPath(import.meta.url));

const SCRAPERS = [
  { name: "Allure", url: "https://www.allure.com/topic/hair", selectors: ["article h2", "article h3", ".summary-item__hed", ".card-title", ".summary-item h2", ".summary-item h3"] },
  { name: "BET", url: "https://www.bet.com/article/category/style-beauty", selectors: ["article h2", "article h3", ".card-title", ".article-title", ".card h2", ".card h3"] },
  { name: "Byrdie", url: "https://www.byrdie.com/hair-4843568", selectors: ["article h2", "article h3", ".card__title", ".mntl-card__title", ".mntl-card h3", ".comp h3"] },
  { name: "Cosmopolitan", url: "https://www.cosmopolitan.com/style-beauty/beauty/g/hair-ideas/", selectors: ["article h2", "article h3", ".full-item-title", ".listicle-slide-hed", ".full-item h2", ".simple-item h2"] },
  { name: "Essence", url: "https://www.essence.com/beauty/hair/", selectors: ["article h2", "article h3", ".card-title", ".entry-title", ".card h2", ".post-item h2"] },
  { name: "Glamour", url: "https://www.glamour.com/topic/hair", selectors: ["article h2", "article h3", ".summary-item__hed", ".card-title", ".summary-item h2", ".summary-item h3"] },
  { name: "NaturallyCurly", url: "https://www.naturallycurly.com/curlreading", selectors: [".article-card h2", ".article-card h3", ".title", ".article-title", ".post-card h2", ".story-card h2", "article h2", "article h3"] },
  { name: "TheCut", url: "https://www.thecut.com/tags/hair/", selectors: ["article h2", "article h3", ".story-title", ".headline", ".story h2", ".feed-item h2", ".river-item h2"] },
  { name: "ExpressWigBraids", url: "https://expresswigbraids.com/blogs/news", selectors: ["article h2", "article h3", ".article-card__title", ".blog-post__title", ".h2", ".blog__post h2"] },
  { name: "LaurenAshtyn", url: "https://thelaurenashtyncollection.com/blogs/blog", selectors: ["article h2", "article h3", ".article-card__title", ".blog-post__title", ".h2", ".blog__post h2"] },
  { name: "LuvmeHair", url: "https://shop.luvmehair.com/blogs/wigs-101", selectors: ["article h2", "article h3", ".article-card__title", ".blog-post__title", ".h2", ".h3", ".blog__post h2"] },
  { name: "MilanoWigs", url: "https://milanowigs.com/blogs/wig-talk-blog", selectors: ["article h2", "article h3", ".article-card__title", ".blog-post__title", ".h2", ".blog__post h2"] },
  { name: "PerfectLocks", url: "https://www.perfectlocks.com/blogs/all-tressed-up", selectors: ["article h2", "article h3", ".article-card__title", ".blog-post__title", ".h3"] },
  { name: "Utress", url: "https://blog.utress.com/best-human-hair-wigs/", selectors: ["article h2", "article h3", ".entry-title", ".post-title", "h2 a", "h3 a", ".entry-title a"] },
  { name: "WigsCom", url: "https://www.wigs.com/blogs/news", selectors: ["article h2", "article h3", ".article-card__title", ".blog-post__title"] },
];

const RESULTS_DIR = join(__dirname, 'verification-results');
const SCRAPER_RESULTS_DIR = join(__dirname, 'TheManeFeed.Tests.Integration', 'test-results');

async function extractTitles(page, selectors) {
  const titles = new Set();
  for (const selector of selectors) {
    try {
      const elements = await page.$$(selector);
      for (const el of elements) {
        const text = (await el.innerText()).trim();
        if (text && text.length > 5 && text.length < 300) {
          titles.add(text);
        }
      }
    } catch { /* selector not found, skip */ }
  }
  // Also try getting all h2/h3 text on the page as a fallback
  try {
    const headings = await page.$$('h2, h3');
    for (const el of headings) {
      const text = (await el.innerText()).trim();
      if (text && text.length > 10 && text.length < 300) {
        titles.add(text);
      }
    }
  } catch { /* skip */ }
  return [...titles];
}

function fuzzyMatch(scraperTitle, pageTitles) {
  const normalizedScraper = scraperTitle.toLowerCase().replace(/[^a-z0-9\s]/g, '').trim();
  if (normalizedScraper.length < 5) return false;

  for (const pageTitle of pageTitles) {
    const normalizedPage = pageTitle.toLowerCase().replace(/[^a-z0-9\s]/g, '').trim();
    // Check if one contains the other (partial match)
    if (normalizedPage.includes(normalizedScraper) || normalizedScraper.includes(normalizedPage)) {
      return true;
    }
    // Check word overlap (at least 60% of words match)
    const scraperWords = normalizedScraper.split(/\s+/).filter(w => w.length > 2);
    const pageWords = normalizedPage.split(/\s+/).filter(w => w.length > 2);
    if (scraperWords.length === 0) continue;
    const matches = scraperWords.filter(w => pageWords.includes(w));
    if (matches.length / scraperWords.length >= 0.6) return true;
  }
  return false;
}

// Process a single scraper
async function verifyScraper(browser, scraper) {
  const result = {
    name: scraper.name,
    url: scraper.url,
    scraperResults: null,
    pageTitles: [],
    matchedItems: [],
    unmatchedItems: [],
    status: 'UNKNOWN',
    error: null,
    itemsScraped: 0,
    itemsMatched: 0,
  };

  // 1. Read scraper test results
  const jsonPath = join(SCRAPER_RESULTS_DIR, `${scraper.name}.json`);
  if (!existsSync(jsonPath)) {
    result.status = 'FAIL';
    result.error = `No test results file found at ${jsonPath}`;
    return result;
  }

  try {
    const data = JSON.parse(await readFile(jsonPath, 'utf-8'));
    result.scraperResults = data;
    result.itemsScraped = data.length;
    if (data.length === 0) {
      result.status = 'FAIL';
      result.error = 'Test results file is empty (0 articles scraped)';
      return result;
    }
  } catch (e) {
    result.status = 'FAIL';
    result.error = `Failed to read/parse test results: ${e.message}`;
    return result;
  }

  // 2. Visit page and extract titles independently
  const page = await browser.newPage();
  try {
    await page.goto(scraper.url, { waitUntil: 'domcontentloaded', timeout: 60000 });
    await page.waitForTimeout(3000);
    result.pageTitles = await extractTitles(page, scraper.selectors);
  } catch (e) {
    result.status = 'FAIL';
    result.error = `Failed to load page: ${e.message}`;
    return result;
  } finally {
    await page.close();
  }

  // 3. Compare scraper results with page titles
  for (const item of result.scraperResults) {
    if (fuzzyMatch(item.Title, result.pageTitles)) {
      result.matchedItems.push(item.Title);
    } else {
      result.unmatchedItems.push(item.Title);
    }
  }

  result.itemsMatched = result.matchedItems.length;
  result.status = result.itemsMatched >= 3 ? 'PASS' : 'FAIL';

  return result;
}

async function main() {
  // Check which scraper name was passed as argument, or run all
  const targetScraper = process.argv[2] || null;

  await mkdir(RESULTS_DIR, { recursive: true });

  const browser = await chromium.launch({ headless: true });

  const scrapersToRun = targetScraper
    ? SCRAPERS.filter(s => s.name === targetScraper)
    : SCRAPERS;

  const allResults = [];

  for (const scraper of scrapersToRun) {
    console.log(`Verifying ${scraper.name}...`);
    const result = await verifyScraper(browser, scraper);
    allResults.push(result);
    console.log(`  ${result.status}: ${result.itemsMatched}/${result.itemsScraped} matched`);
    if (result.error) console.log(`  Error: ${result.error}`);

    // Save individual result
    await writeFile(
      join(RESULTS_DIR, `${scraper.name}.json`),
      JSON.stringify(result, null, 2)
    );
  }

  // Save summary
  await writeFile(
    join(RESULTS_DIR, 'summary.json'),
    JSON.stringify(allResults, null, 2)
  );

  await browser.close();

  // Print summary
  const passed = allResults.filter(r => r.status === 'PASS').length;
  const failed = allResults.filter(r => r.status === 'FAIL').length;
  console.log(`\n=== SUMMARY ===`);
  console.log(`Total: ${allResults.length} | Passed: ${passed} | Failed: ${failed}`);
  for (const r of allResults) {
    console.log(`  ${r.status === 'PASS' ? 'PASS' : 'FAIL'} ${r.name} (${r.itemsMatched}/${r.itemsScraped} matched)`);
  }
}

main().catch(console.error);
