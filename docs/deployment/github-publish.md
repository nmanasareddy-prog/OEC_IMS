# Local Run vs GitHub Public Deploy

## Will pushing to GitHub open my page?

| What you push | What happens today |
|---------------|-------------------|
| **Without setup below** | CI builds only — **no public URL** |
| **After GitHub Pages setup** | Every push to `main` deploys the **UI** to a public URL |

GitHub Pages hosts **static files only** (your React build). The **.NET API does not run on GitHub Pages**. For login and data to work in the browser, you also need the API on a host (Render, Railway, Azure, etc.) and set `VITE_API_URL`.

---

## Local testing

| Action | How |
|--------|-----|
| **One-click start** | Double-click `start-oec-ims.bat` at repo root |
| API | http://localhost:5083 |
| UI | http://localhost:5173 |
| Login | `admin` / `Admin123!` or `clerk` / `Clerk123!` |

---

## GitHub Pages — UI auto-deploy on push

### 1. One-time GitHub repo settings

1. Push this repo to GitHub (branch **`main`**).
2. Open the repo on GitHub → **Settings** → **Pages**.
3. Under **Build and deployment** → **Source**, choose **GitHub Actions**.
4. Add a repository variable:
   - Name: `VITE_API_URL`
   - Value: your **permanent** API root, e.g. `https://oec-ims-api.onrender.com` (no trailing slash)

### 2. Push to `main`

```bash
git add .
git commit -m "Enable GitHub Pages deploy"
git push origin main
```

### 3. Check the workflow

- **Actions** tab → workflow **Deploy Frontend (GitHub Pages)** → green checkmark.
- **Settings** → **Pages** shows the site URL.

Your UI will be at:

```text
https://<your-github-username>.github.io/<repo-name>/
```

Example: repo `OEC_IMS` → `https://jane.github.io/OEC_IMS/`

### 4. If the page is blank or 404

- Confirm **Pages** source is **GitHub Actions**.
- Confirm the workflow used `VITE_BASE_PATH: /<repo-name>/`.
- Hard-refresh the browser (Ctrl+F5).

---

## Permanent backend configuration

The frontend now uses the following rules:

- **Local development:** uses the Vite proxy to `http://localhost:5083`
- **Production builds:** require `VITE_API_URL` to be set explicitly

> Current production tunnel: `https://oec-ims-api-demo.loca.lt`

Set the repository variable at:

- **GitHub** → **Settings** → **Secrets and variables** → **Actions** → **Variables**
- Name: `VITE_API_URL`
- Value: your permanent backend URL, e.g. `https://oec-ims-api-demo.loca.lt`

If you want to test a production build locally, update `frontend/.env.production` to your permanent backend URL and rebuild.

---

## API hosting options

Pick one host and deploy `backend/src/OEC.IMS.Api` to a **permanent** URL.

| Host | Notes |
|------|--------|
| [Render](https://render.com) | Good for demos |
| [Railway](https://railway.app) | Simple .NET deployment |
| Azure App Service | Good for production-style hosting |

On the API host:

1. Run EF migrations / seed.
2. Set `Jwt:SigningKey` and **CORS** to allow your Pages URL, e.g. `https://<user>.github.io`.
3. Set `VITE_API_URL` to the permanent API URL.
4. Push `main` again so the SPA rebuilds with the new API base.

---

## README live demo links

After Pages is live, add to the root `README.md`:

```markdown
## Live demo

- **UI:** https://<username>.github.io/<repo-name>/
- **API:** https://<your-api-host>/swagger
```
