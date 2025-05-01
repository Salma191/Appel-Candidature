// reclamation.component.ts
import {
  AfterViewInit,
  Component,
  ElementRef,
  ViewChild,
} from '@angular/core';
import { PosteService } from '../../services/poste.service';
import { OrgChart } from 'd3-org-chart';
import 'd3-flextree';
import { SidebarComponent } from '../../layout/sidebar/sidebar.component';
import { TableComponent } from '../../layout/table/table.component';

interface OrgUnit {
  id: number;
  sigle: string;
  rattachementHierarchique: string | null;
  poste: string;
  typePoste: string | null;
  localite: string;
  titulaire: string;
  dateNomination: string;
}

@Component({
  selector: 'app-reclamation',
  standalone: true,
  imports: [SidebarComponent,
    TableComponent
  ],
  templateUrl: './reclamation.component.html',
  styleUrls: ['./reclamation.component.scss']
})
export class ReclamationComponent implements AfterViewInit {
  @ViewChild('chartContainer', { static: true })
  container!: ElementRef<HTMLDivElement>;

  // Déclare la propriété chart
  private chart!: OrgChart<OrgUnit>;

  constructor(private posteService: PosteService) {}

  ngAfterViewInit(): void {
    this.posteService.getOrg().subscribe((orgData: OrgUnit[]) => {
      // 1) Normalisation des sigles
      orgData.forEach(d => {
        d.sigle = d.sigle.trim().toUpperCase();
        if (d.rattachementHierarchique) {
          d.rattachementHierarchique = d.rattachementHierarchique.trim().toUpperCase();
        }
      });

      // 2) Map des sigles vers les noeuds
      const sigleToNode = new Map<string, OrgUnit>();
      orgData.forEach(d => sigleToNode.set(d.sigle, d));

      // 3) Construction du parentMap
      const parentMap = new Map<string, string | undefined>();
      orgData.forEach(d => {
        parentMap.set(
          d.sigle,
          d.rattachementHierarchique && sigleToNode.has(d.rattachementHierarchique)
            ? d.rattachementHierarchique
            : undefined
        );
      });

      // 4) Détection et bris de cycles
      function hasCycle(start: string): boolean {
        const seen = new Set<string>();
        let cur: string | undefined = start;
        while (cur) {
          if (seen.has(cur)) return true;
          seen.add(cur);
          cur = parentMap.get(cur);
        }
        return false;
      }
      parentMap.forEach((parent, child) => {
        if (parent && hasCycle(child)) {
          parentMap.set(child, undefined);
        }
      });

      // ─── 4.5) Réparer le problème de "multiple roots" ────────────────────────────
    // on récupère tous les sigles sans parent
    const roots = orgData
    .filter(d => parentMap.get(d.sigle) === undefined)
    .map(d => d.sigle);
  console.warn('⚠️ racines trouvées :', roots);

  // on choisit DG comme unique racine et on rattache toutes les autres sous DG
  const uniqueRoot = 'DG';
  roots.forEach(sigle => {
    if (sigle !== uniqueRoot) {
      parentMap.set(sigle, uniqueRoot);
    }
  });

      // 5) Initialisation de l'OrgChart
      this.chart = new OrgChart<OrgUnit>()
        // on cast en any pour accéder à node.data
        .nodeId((node: any) => node.sigle)
        .parentNodeId((node: any) => parentMap.get(node.sigle))
        .nodeHeight(() => 110)
        .nodeWidth(() => 240)
        .childrenMargin(() => 40)
        .compactMarginBetween(() => 20)
        .compactMarginPair(() => 20)
        .neighbourMargin(() => 20)
        .nodeContent((node: any) => {
          const d = node.data as OrgUnit;
          const date = new Date(d.dateNomination).toLocaleDateString('fr-FR');
          return `
            <div style="padding:8px;border-radius:8px;border:1px solid #ccc;background:#fff">
              <strong style="font-size:14px">${d.poste}</strong><br>
              <em style="font-size:12px;color:#666">${d.titulaire}</em><br>
              <small style="font-size:11px">
                Sigle : ${d.sigle}<br>
                Lieu : ${d.localite}<br>
                Nommé(e) : ${date}
              </small>
            </div>
          `;
        })
        // on passe l'élément DOM, pas un sélecteur
        .container('#chartContainer')
        .data(orgData)
        .render();
    });
  }
}
