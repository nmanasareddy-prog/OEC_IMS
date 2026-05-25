# Local Run vs GitHub Public Deploy

## Will pushing to GitHub open my page?

| What you push | What happens today |
|---------------|-------------------|
| **Without setup below** | CI builds only — **no public URL** |
| **After GitHub Pages setup** | Every push to `main` deploys the **UI** to a public URL |

GitHub Pages hosts **static files only** (your React build). The **.NET API does not run on GitHub Pages**. For login and data to work in the browser, you also need the API on a host (Render, Railway, Azure, etc.) and set `VITE_API_URL` (see below).

---

## Local testing

| Action | How |
|--------|-----|
| **One-click start** | Double-click `start-oec-ims.bat` at repo root |
| API | http://localhost:5083 |
| UI | http://localhost:5173 |
| Login | `admin` / `Admin123!` or `clerk` / `Clerk123!` |

---

## GitHub Pages — UI auto-deploy on push (recommended first step)

### 1. One-time GitHub repo settings

1. Push this repo to GitHub (branch **`main`**).
2. Open the repo on GitHub → **Settings** → **Pages**.
3. Under **Build and deployment** → **Source**, choose **GitHub Actions** (not “Deploy from a branch”).
4. (Optional, for live API calls from the deployed UI)  
   **Settings** → **Secrets and variables** → **Actions** → **Variables** → **New repository variable**  
   - Name: `VITE_API_URL`  
   - Value: your public API root, e.g. `https://oec-ims-api.onrender.com` (no trailing slash)

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

Each push to `main` rebuilds and updates that URL (usually within 1–3 minutes).

### 4. If the page is blank or 404

- Confirm **Pages** source is **GitHub Actions**.
- Confirm the workflow used `VITE_BASE_PATH: /<repo-name>/` (handled automatically in `deploy-frontend-pages.yml`).
- Hard-refresh the browser (Ctrl+F5).

---

## API for a full live demo (second step)

GitHub Pages cannot run the .NET API. Pick one host and deploy `backend/src/OEC.IMS.Api`:

| Host | Notes |
|------|--------|
| [Render](https://render.com) | Free tier, good for demos |
| [Railway](https://railway.app) | Simple .NET deploy |
| Azure App Service | Closer to enterprise |

On the API host:

1. Run EF migrations / seed.
2. Set `Jwt:SigningKey` and **CORS** to allow your Pages URL, e.g. `https://<user>.github.io`.
3. Set GitHub variable `VITE_API_URL` to that API URL and push `main` again (rebuilds the SPA with the correct API).

Until `VITE_API_URL` points to a live API, the **page will open** but login/API calls will fail (expected).

### Single-URL option (advanced)

Build the SPA and copy `frontend/dist` into the API `wwwroot` so one URL serves UI + API — simpler for recruiters, one deploy instead of two.

---

## Workflows in this repo

| Workflow | Trigger | Result |
|----------|---------|--------|
| `ci-frontend.yml` | push/PR to `main` | Build + lint only |
| `ci-backend.yml` | push/PR to `main` | Build + test only |
| **`deploy-frontend-pages.yml`** | **push to `main`** | **Publishes UI to GitHub Pages** |

---

## README live demo links

After Pages is live, add to the root `README.md`:

```markdown
## Live demo

- **UI:** https://<username>.github.io/<repo-name>/
- **API:** https://<your-api-host>/swagger
```
