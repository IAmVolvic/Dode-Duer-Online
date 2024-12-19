import { useCookies } from 'react-cookie';
import { setAuth } from './useAuthentication';
import { Api, UserLoginRequestDTO } from '@Api';
import { ErrorToast } from '@components/errorToast';
import { useEffect, useState } from 'react';

export const useLogin = (data: UserLoginRequestDTO) => {
  const [success, setSuccess] = useState(false);
  const api = new Api();
  const [, setCookie] = useCookies(['Authentication']);

  const login = () => {
    api.user.userPLogin(data).then((res) => { 
      setCookie('Authentication', res.data.jwt, { path: '/', expires: new Date(Date.now() + 1000*60*60*24*7) });
        setAuth();
        window.location.href = '/';
    }).catch((err) => {
      ErrorToast(err);
    });
  };
  
  return login;
};