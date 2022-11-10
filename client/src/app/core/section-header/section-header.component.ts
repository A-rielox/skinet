import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
   selector: 'app-section-header',
   templateUrl: './section-header.component.html',
   styleUrls: ['./section-header.component.scss'],
})
export class SectionHeaderComponent implements OnInit {
   breadcrumb$: Observable<any[]>;

   constructor(private bcService: BreadcrumbService) {}

   ngOnInit(): void {
      this.breadcrumb$ = this.bcService.breadcrumbs$;
      // el " | async " hace el unsubscribe cuando se destruye el componente
   }
}

// yellow 🟡 en los estilos globales sobreescribo los que tiene breadcrumb xdefecto ( en style.scss )
