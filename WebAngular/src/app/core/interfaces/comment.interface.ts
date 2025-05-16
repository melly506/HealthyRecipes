import { SystemFields } from './system.interface';
import { User } from './users.interface';

export interface UserComment extends SystemFields {
  id: string;
  text: string;
  recipeId: string;
  user: User;
}

export interface CommentForManage {
  text: string;
}
