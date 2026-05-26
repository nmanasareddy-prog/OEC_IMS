import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { AppProviders } from '@/app/providers/AppProviders';
import { AppRouter } from '@/app/routes/AppRouter';
import './index.css';

const restoreDeepLink = () => {
  const params = new URLSearchParams(window.location.search);
  const route = params.get('p');

  if (!route) {
    return;
  }

  const normalizedRoute = decodeURIComponent(route);
  const base = import.meta.env.BASE_URL.replace(/\/$/, '');
  const target = `${base}${normalizedRoute}`;

  window.history.replaceState({}, '', target);
};

restoreDeepLink();

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <AppProviders>
      <AppRouter />
    </AppProviders>
  </StrictMode>,
);
