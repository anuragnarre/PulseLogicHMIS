
export class PulseLogicRoute {

    public RouteId: number = 0;
    public UrlFullPath: string = null;
    public DisplayName: string = null;
    public PermissionId: number = null;
    public ParentRouteId: number = null;    
    public DefaultShow: boolean = null;
    public RouterLink: string = null;
    public Css: string = "";
    public DisplaySeq: number = null;
    public ChildRoutes: Array<PulseLogicRoute> = new Array<PulseLogicRoute>();
    public ChildRoutesDefaultShowCount: number = 0;
    public IsSecondaryNavInDropdown:boolean=false;

}
