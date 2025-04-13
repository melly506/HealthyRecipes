export interface User {
  id: string;
  identifier: string;
  firstName: string;
  lastName: string;
  email: string;
  username: string;
  bio: string;
  gender: string;
  picture: string;
}

export interface UserForUpdate {
  firstName: string;
  lastName: string;
  bio: string;
  gender: string;
  picture: string;
}
