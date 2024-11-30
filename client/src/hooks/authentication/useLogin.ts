import { useCookies } from 'react-cookie';
import { setAuth } from './useAuthentication';
import { Api, UserLoginRequestDTO } from '@Api';
import { toast } from "react-hot-toast";

export const useLogin = (data: UserLoginRequestDTO) => {
  const api = new Api();
  const [, setCookie] = useCookies(['Authentication']);

  const login = () => {
    api.user.userPLogin(data).then((res) => { 
      setCookie('Authentication', res.data.jwt, { path: '/', expires: new Date(Date.now() + 1000*60*60*24*7) });
      setAuth();
    }).catch((err) => {
      if (err.response && err.response.data && err.response.data.errors) {
        let errorMessage = '';
        Object.keys(err.response.data.errors).forEach((field) => {
          err.response.data.errors[field].forEach((message) => {
            errorMessage += `${field}: ${message}\n`; 
          });
        });

        toast.error(errorMessage);
      }
    });
  };
  
  return login;
};