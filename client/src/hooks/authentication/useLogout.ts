import { useCookies } from 'react-cookie';
import { clearAuth } from './useAuthentication';

export const useLogout = () => {
  const [, setCookie] = useCookies(['Authentication']);
  const logout = () => {
    setCookie('Authentication', '', { domain: ".xn--xck.dev", path: '/', expires: new Date(0) });
    clearAuth();
  };
  
  return logout;
};