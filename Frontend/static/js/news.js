document.addEventListener('DOMContentLoaded', () => {
  // --- Проверка авторизации и обновление navbar ---
  const username = localStorage.getItem('username');
  const jwt = localStorage.getItem('jwt_token');
  const navLinks = document.getElementById('nav-links');

  if (username && jwt && navLinks) {
    updateNavbarForLoggedInUser(username);
  }

  function updateNavbarForLoggedInUser(username) {
    const navLinks = document.getElementById('nav-links');
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

  // --- Получение и вывод новостей ---
  fetch('http://127.0.0.1:5000/api/news')
    .then(response => response.json())
    .then(data => {
      if (data.articles) {
        renderNews(data.articles);
      } else {
        console.error('No articles found');
      }
    })
    .catch(err => {
      console.error('Failed to fetch news:', err);
    });

  function renderNews(articles) {
    const newsSection = document.querySelector('.news-section');
    if (!newsSection) return;

    const list = document.createElement('ul');
    list.classList.add('news-list');

    articles.forEach(article => {
      const item = document.createElement('li');
      item.classList.add('news-item');

      item.innerHTML = `
        <a href="${article.url}" target="_blank" rel="noopener noreferrer">
          <h3>${article.title}</h3>
          <p><small>${new Date(article.publishedAt).toLocaleString()} | ${article.source}</small></p>
        </a>
      `;

      list.appendChild(item);
    });

    newsSection.appendChild(list);
  }
});
