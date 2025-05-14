import { Component, DestroyRef, inject, Input } from '@angular/core';
import { BehaviorSubject, distinctUntilChanged, filter, map, mergeMap } from 'rxjs';
import { UsersService } from '../../core/services';
import { User } from '../../core/interfaces';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-created-by-wrapper',
  standalone: true,
  imports: [],
  templateUrl: './created-by-wrapper.component.html',
  styleUrl: './created-by-wrapper.component.scss'
})
export class CreatedByWrapperComponent {
  @Input() createdBy?: string = '';

  createdByUser: User | null = null;

  #usersService = inject(UsersService);
  #dr = inject(DestroyRef);
  #createdById$ = new BehaviorSubject<string>('');

  ngOnInit(): void {
    this.#createdById$
      .pipe(
        filter(Boolean),
        distinctUntilChanged(),
        mergeMap(createdById => this.#usersService
          .getUsers(`identifier== "${createdById}"`)),
        map((users: User[]) => Array.isArray(users) ? users[0] : null),
        filter(Boolean),
        takeUntilDestroyed(this.#dr)
      )
      .subscribe(createdBy => {
        this.createdByUser = createdBy;
      });
  }

  ngOnChanges(): void {
    this.#createdById$.next(this.createdBy || '');
  }
}
