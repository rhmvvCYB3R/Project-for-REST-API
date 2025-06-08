const API_BASE = 'http://localhost:5206/api/ExchangeRate';
const navLinks = document.getElementById('nav-links');
const token = localStorage.getItem('jwt_token');
const username = localStorage.getItem('username');


if (token && username) {
    updateNavbarForLoggedInUser(username);
} else {
    console.warn("User is not logged in.");
}

function updateNavbarForLoggedInUser(username) {
    navLinks.innerHTML = `
        <span class="greeting">Hi, <a href="profile.html" class="profile-link">${username}</a></span>
        <button id="logout-btn" class="logout-btn">Logout</button>
    `;

    const logoutBtn = document.getElementById('logout-btn');
    logoutBtn.addEventListener('click', () => {
        localStorage.removeItem('jwt_token');
        localStorage.removeItem('username');
        window.location.href = 'index.html';
    });
}

// Словарь валют с эмодзи-флагами
const currencyEmojiFlags = {
  USD: "🇺🇸",
  AED: "🇦🇪",
  AFN: "🇦🇫",
  ALL: "🇦🇱",
  AMD: "🇦🇲",
  ANG: "🇧🇶",  // Карибские Нидерланды (приближённо)
  AOA: "🇦🇴",
  ARS: "🇦🇷",
  AUD: "🇦🇺",
  AWG: "🇦🇼",
  AZN: "🇦🇿",
  BAM: "🇧🇦",
  BBD: "🇧🇧",
  BDT: "🇧🇩",
  BGN: "🇧🇬",
  BHD: "🇧🇭",
  BIF: "🇧🇮",
  BMD: "🇧🇲",
  BND: "🇧🇳",
  BOB: "🇧🇴",
  BRL: "🇧🇷",
  BSD: "🇧🇸",
  BTN: "🇧🇹",
  BWP: "🇧🇼",
  BYN: "🇧🇾",
  BZD: "🇧🇿",
  CAD: "🇨🇦",
  CDF: "🇨🇩",
  CHF: "🇨🇭",
  CLP: "🇨🇱",
  CNY: "🇨🇳",
  COP: "🇨🇴",
  CRC: "🇨🇷",
  CUP: "🇨🇺",
  CVE: "🇨🇻",
  CZK: "🇨🇿",
  DJF: "🇩🇯",
  DKK: "🇩🇰",
  DOP: "🇩🇴",
  DZD: "🇩🇿",
  EGP: "🇪🇬",
  ERN: "🇪🇷",
  ETB: "🇪🇹",
  EUR: "🇪🇺",
  FJD: "🇫🇯",
  FKP: "🇫🇰",
  FOK: "",       // Нет отдельного флага (Фарерские острова используют DK)
  GBP: "🇬🇧",
  GEL: "🇬🇪",
  GGP: "🇬🇬",
  GHS: "🇬🇭",
  GIP: "🇬🇮",
  GMD: "🇬🇲",
  GNF: "🇬🇳",
  GTQ: "🇬🇹",
  GYD: "🇬🇾",
  HKD: "🇭🇰",
  HNL: "🇭🇳",
  HRK: "🇭🇷",
  HTG: "🇭🇹",
  HUF: "🇭🇺",
  IDR: "🇮🇩",
  ILS: "🇮🇱",
  IMP: "🇮🇲",
  INR: "🇮🇳",
  IQD: "🇮🇶",
  IRR: "🇮🇷",
  ISK: "🇮🇸",
  JEP: "🇯🇪",
  JMD: "🇯🇲",
  JOD: "🇯🇴",
  JPY: "🇯🇵",
  KES: "🇰🇪",
  KGS: "🇰🇬",
  KHR: "🇰🇭",
  KID: "🇰🇮",
  KMF: "🇰🇲",
  KRW: "🇰🇷",
  KWD: "🇰🇼",
  KYD: "🇰🇾",
  KZT: "🇰🇿",
  LAK: "🇱🇦",
  LBP: "🇱🇧",
  LKR: "🇱🇰",
  LRD: "🇱🇷",
  LSL: "🇱🇸",
  LYD: "🇱🇾",
  MAD: "🇲🇦",
  MDL: "🇲🇩",
  MGA: "🇲🇬",
  MKD: "🇲🇰",
  MMK: "🇲🇲",
  MNT: "🇲🇳",
  MOP: "🇲🇴",
  MRU: "🇲🇷",
  MUR: "🇲🇺",
  MVR: "🇲🇻",
  MWK: "🇲🇼",
  MXN: "🇲🇽",
  MYR: "🇲🇾",
  MZN: "🇲🇿",
  NAD: "🇳🇦",
  NGN: "🇳🇬",
  NIO: "🇳🇮",
  NOK: "🇳🇴",
  NPR: "🇳🇵",
  NZD: "🇳🇿",
  OMR: "🇴🇲",
  PAB: "🇵🇦",
  PEN: "🇵🇪",
  PGK: "🇵🇬",
  PHP: "🇵🇭",
  PKR: "🇵🇰",
  PLN: "🇵🇱",
  PYG: "🇵🇾",
  QAR: "🇶🇦",
  RON: "🇷🇴",
  RSD: "🇷🇸",
  RUB: "🇷🇺",
  RWF: "🇷🇼",
  SAR: "🇸🇦",
  SBD: "🇸🇧",
  SCR: "🇸🇨",
  SDG: "🇸🇩",
  SEK: "🇸🇪",
  SGD: "🇸🇬",
  SHP: "🇸🇭",
  SLE: "🇸🇱",
  SLL: "🇸🇱",
  SOS: "🇸🇴",
  SRD: "🇸🇷",
  SSP: "🇸🇸",
  STN: "🇸🇹",
  SYP: "🇸🇾",
  SZL: "🇸🇿",
  THB: "🇹🇭",
  TJS: "🇹🇯",
  TMT: "🇹🇲",
  TND: "🇹🇳",
  TOP: "🇹🇴",
  TRY: "🇹🇷",
  TTD: "🇹🇹",
  TVD: "🇹🇻",
  TWD: "🇹🇼",
  TZS: "🇹🇿",
  UAH: "🇺🇦",
  UGX: "🇺🇬",
  UYU: "🇺🇾",
  UZS: "🇺🇿",
  VES: "🇻🇪",
  VND: "🇻🇳",
  VUV: "🇻🇺",
  WST: "🇼🇸",
  XAF: "",   // Специальная зона (КФАФ), без флага
  XCD: "",   // Восточно-карибский доллар, нет флага страны
  XCG: "",
  XDR: "",
  XOF: "",
  XPF: "",
  YER: "🇾🇪",
  ZAR: "🇿🇦",
  ZMW: "🇿🇲",
  ZWL: "🇿🇼",
};


async function fetchCurrencies(baseCurrency = 'USD') {
    try {
        const url = new URL(`${API_BASE}/rates`);
        url.searchParams.append('baseCurrency', baseCurrency);

        const response = await fetch(url, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Accept': 'application/json'
            }
        });

        if (!response.ok) {
            const text = await response.text();
            throw new Error(`Error ${response.status}: ${text || 'empty response'}`);
        }

        const data = await response.json();

        if (data.result !== 'success' || !data.conversion_rates) {
            throw new Error('Invalid server response');
        }

        return Object.keys(data.conversion_rates);

    } catch (err) {
        console.error("Error loading currencies:", err);
        alert("Failed to load currencies list. Maybe you are not authorized.");
        return [];
    }
}

async function populateCurrencies() {
    const currencies = await fetchCurrencies();

    const fromSelect = document.getElementById('fromCurrency');
    const toSelect = document.getElementById('toCurrency');

    if (!currencies.length) {
        fromSelect.innerHTML = '<option disabled>Error loading currencies</option>';
        toSelect.innerHTML = '<option disabled>Error loading currencies</option>';
        return;
    }

    fromSelect.innerHTML = '';
    toSelect.innerHTML = '';

    currencies.forEach(cur => {
        const flag = currencyEmojiFlags[cur] || '';
        const option1 = document.createElement('option');
        option1.value = cur;
        option1.textContent = `${flag} ${cur}`;

        const option2 = option1.cloneNode(true);

        fromSelect.appendChild(option1);
        toSelect.appendChild(option2);
    });

    fromSelect.value = 'USD';
    toSelect.value = 'EUR';
}

async function convertCurrency() {
    const from = document.getElementById('fromCurrency').value;
    const to = document.getElementById('toCurrency').value;
    const amount = parseFloat(document.getElementById('amount').value);
    const resultDiv = document.getElementById('result');

    if (!amount || amount <= 0) {
        resultDiv.textContent = "Please enter a valid amount.";
        return;
    }

    try {
        const url = new URL(`${API_BASE}/rates`);
        url.searchParams.append('baseCurrency', from);

        const response = await fetch(url, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Accept': 'application/json'
            }
        });

        if (!response.ok) {
            const errorText = await response.text();
            resultDiv.textContent = `Request error: ${response.status} - ${errorText}`;
            return;
        }

        const data = await response.json();

        if (data.result !== 'success' || !data.conversion_rates) {
            resultDiv.textContent = "Invalid server response on conversion.";
            return;
        }

        const rate = data.conversion_rates[to];
        if (!rate) {
            resultDiv.textContent = `Exchange rate for currency "${to}" not found.`;
            return;
        }

        const convertedAmount = amount * rate;
        resultDiv.textContent = `${amount.toFixed(2)} ${from} = ${convertedAmount.toFixed(2)} ${to}`;

    } catch (err) {
        console.error("Conversion error:", err);
        resultDiv.textContent = "Error during request.";
    }
}

document.getElementById('convertBtn').addEventListener('click', convertCurrency);

window.onload = populateCurrencies;
