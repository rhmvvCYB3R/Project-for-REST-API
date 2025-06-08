document.addEventListener('DOMContentLoaded', () => {
    const loginForm = document.getElementById('login-form');
    const registerForm = document.getElementById('register-form');

    // backend link
    const API_BASE_URL = 'http://localhost:5206/api/Auth';

    // register handler
    registerForm.addEventListener('submit', async (e) => {
        e.preventDefault();

        const username = document.getElementById('register-username').value.trim();
        const password = document.getElementById('register-password').value.trim();

        try {
            const response = await fetch(`${API_BASE_URL}/register`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ username, password })
            });

            if (response.ok) {
                alert('Registration successful! You can now log in.');
                document.getElementById('login-tab').click();
            } else {
                const error = await response.text();
                alert('Registration failed: ' + error);
            }
        } catch (error) {
            console.error('Registration error:', error);
            alert('Registration error. See console for details.');
        }
    });

    // login handler
    loginForm.addEventListener('submit', async (e) => {
        e.preventDefault();

        const username = document.getElementById('login-username').value.trim();
        const password = document.getElementById('login-password').value.trim();

        try {
            const response = await fetch(`${API_BASE_URL}/login`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ username, password })
            });

            if (response.ok) {
                const data = await response.json();
                
                // Save JWT and username
                localStorage.setItem('jwt_token', data.token);
                localStorage.setItem('username', username);

                alert('Login successful!');
                window.location.href = 'index.html';
            } else {
                const error = await response.text();
                alert('Login failed: ' + error);
            }
        } catch (error) {
            console.error('Login error:', error);
            alert('Login error. See console for details.');
        }
    });
});
