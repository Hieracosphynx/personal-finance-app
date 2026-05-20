const params = new URLSearchParams(window.location.search);
const linkToken = params.get('link_token');

const handler = Plaid.create({
  token: linkToken,
  onSuccess: async (publicToken, metadata) => {
    await fetch('/plaid/exchange-token', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        publicToken,
        institutionName: metadata.institution.name
      })
    });
    document.body.innerHTML = '<h2>Account connected! You can close this tab!</h2>';
  },
  onExit: (err) => {
    if (err) console.error(err);
  }
});

document.getElementById('link-btn').onclick = () => handler.open();
