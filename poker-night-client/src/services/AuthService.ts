import jwt_decode from 'jwt-decode';

export default class AuthService {
  SaveAuthToken (params: string): void {
    const jwtMatch = params.match(/access_token=([^&]*)/);
    const tokenTypeMatch = params.match(/token_type=([^&]*)/);
    if (jwtMatch && tokenTypeMatch) {
      localStorage.setItem('auth_header', `${tokenTypeMatch[1]} ${jwtMatch[1]}`);

      const decodedJwt: any = jwt_decode(jwtMatch[1]);
      localStorage.setItem('loginid', decodedJwt.sub);
      if (decodedJwt["cognito:groups"]) {
        localStorage.setItem('admin', decodedJwt["cognito:groups"].includes("Admin"));
      }
    }

    const expiryMatch = params.match(/expires_in=([^&]*)/);
    if (expiryMatch) localStorage.setItem('jwt_expiry', expiryMatch[1]);
  }

  LogOut () {
    localStorage.removeItem('auth_header');
    localStorage.removeItem('loginid');
    localStorage.removeItem('admin');
    localStorage.removeItem('jwt_expiry');
  }

  IsLoggedIn (): boolean {
    var auth = localStorage.getItem('auth_header');
    return auth != null && auth != '';
  }

  IsAdmin (): boolean {
    var admin = localStorage.getItem('admin');
    return admin === 'true';
  }

  CurrentLoginId (): string | null {
    return localStorage.getItem('loginid');
  }

  AuthHeader (): string {
    return localStorage.getItem('auth_header') ?? '';
  }
}
