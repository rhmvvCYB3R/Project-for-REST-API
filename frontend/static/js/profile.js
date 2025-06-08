document.addEventListener('DOMContentLoaded', () => {
  const username = localStorage.getItem('username');
  const jwt = localStorage.getItem('jwt_token');
  const navLinks = document.getElementById('nav-links');

  if (username && jwt && navLinks) {
    updateNavbarForLoggedInUser(username);
  }

  const currentUsername = document.getElementById('current-username');
  if (currentUsername && username) {
    currentUsername.textContent = username;
  }

  const updateBtn = document.getElementById('update-btn');
  if (updateBtn) {
    updateBtn.addEventListener('click', async () => {
      const newUsername = document.getElementById('new-username').value;
      const newPassword = document.getElementById('new-password').value;

      if (!newUsername || !newPassword) {
        alert('Both fields are required.');
        return;
      }

      const response = await fetch('http://localhost:5206/api/Auth/update', {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${jwt}`
        },
        body: JSON.stringify({
          username: newUsername,
          password: newPassword
        })
      });

      if (response.ok) {
        alert('Profile updated successfully!');
        localStorage.setItem('username', newUsername);
        location.reload();
      } else {
        alert('Failed to update profile.');
      }
    });
  }

  const deleteBtn = document.getElementById('delete-btn');
  if (deleteBtn) {
    deleteBtn.addEventListener('click', async () => {
      if (!confirm('Are you sure you want to delete your account? This action cannot be undone.')) {
        return;
      }

      const response = await fetch('http://localhost:5206/api/Auth/delete', {
        method: 'DELETE',
        headers: {
          Authorization: `Bearer ${jwt}`
        }
      });

      if (response.ok) {
        alert('Account deleted.');
        localStorage.clear();
        window.location.href = 'index.html';
      } else {
        alert('Failed to delete account.');
      }
    });
  }
});

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
