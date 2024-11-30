import { useCookies } from 'react-cookie';
import { clearAuth } from './useAuthentication';

export const useLogout = () => {
  const [, setCookie] = useCookies(['Authentication']);
  const logout = () => {
    setCookie('Authentication', '', { path: '/', expires: new Date(0) });
    clearAuth();
  };
  
  return logout;
};