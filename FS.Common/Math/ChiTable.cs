
using System;

namespace FS.Common.Math
{

    public static class ChiTable
    {
        //Math.Round (.0151765475 * 1000D,0) = 15
        private static double[] m_objArrayOfDoubleChiDist = new double[]
        {
            0.0D,
            0.0252271206281628D,
            0.0356705917246491D,
            0.0436800960816589D,
            0.0504290288456096D,
            0.0563719777933221D,
            0.0617421240310736D,
            0.0666780117041083D,
            0.0712699254302653D,
            0.0755805878079022D,
            0.0796556745480493D,
            0.0835296729839268D,
            0.0872293808958702D,
            0.0907761127737335D,
            0.0941871532623296D,
            0.0974767498107936D,
            0.10065681142515D,
            0.103737413362658D,
            0.10672716983906D,
            0.109633514636677D,
            0.11246291599204D,
            0.115221043651828D,
            0.117912900501D,
            0.12054292753562D,
            0.123115088495661D,
            0.12563293877734D,
            0.128099682053515D,
            0.13051821718045D,
            0.13289117735348D,
            0.135220963021992D,
            0.137509769738147D,
            0.139759611861147D,
            0.141972343002813D,
            0.144149672885034D,
            0.146293183302939D,
            0.148404340676793D,
            0.150484507624335D,
            0.152534952978085D,
            0.154556860592503D,
            0.15655133711505D,
            0.158519418866376D,
            0.160462077951388D,
            0.162380227703762D,
            0.164274727550685D,
            0.166146387371549D,
            0.167995971413517D,
            0.169824201817803D,
            0.171631761802975D,
            0.173419298545193D,
            0.175187425789927D,
            0.176936726225148D,
            0.178667753642083D,
            0.18038103490634D,
            0.182077071759355D,
            0.183756342467686D,
            0.185419303335573D,
            0.187066390094375D,
            0.188698019180925D,
            0.190314588915455D,
            0.191916480588592D,
            0.193504059465852D,
            0.195077675717156D,
            0.196637665278109D,
            0.198184350649066D,
            0.199718041637388D,
            0.201239036047751D,
            0.20274762032489D,
            0.20424407015272D,
            0.205728651013398D,
            0.207201618709564D,
            0.208663219852666D,
            0.21011369232004D,
            0.211553265683148D,
            0.212982161609166D,
            0.214400594237929D,
            0.215808770536054D,
            0.217206890629902D,
            0.230674713993703D,
            0.243316354787341D,
            0.255255138491598D,
            0.266586029640319D,
            0.277384027612933D,
            0.287709612295587D,
            0.297612410206109D,
            0.30713375628658D,
            0.316308519649003D,
            0.325166444757921D,
            0.333733153796063D,
            0.342030909777852D,
            0.350079206763503D,
            0.357895232808695D,
            0.365494237692414D,
            0.372889828342063D,
            0.380094208624401D,
            0.387118375808981D,
            0.393972282915565D,
            0.400664973926965D,
            0.407204697219641D,
            0.413599001358877D,
            0.41985481650283D,
            0.42597852397665D,
            0.431976019493497D,
            0.437852751755939D,
            0.443613785826884D,
            0.44926383035944D,
            0.454807274580065D,
            0.460248218299498D,
            0.465590498374614D,
            0.470837712122135D,
            0.475993238103569D,
            0.481060254634143D,
            0.486041756313816D,
            0.490940568833351D,
            0.495759362270983D,
            0.500500663064082D,
            0.505166864814113D,
            0.5097602380613D,
            0.514282939146907D,
            0.518737018265418D,
            0.523124426795588D,
            0.527447023987999D,
            0.531706583077048D,
            0.535904796876985D,
            0.540043282914413D,
            0.544123588143503D,
            0.548147193284797D,
            0.552115516823804D,
            0.556029918701573D,
            0.559891703725852D,
            0.563702124728359D,
            0.567462393132402D,
            0.571173652040711D,
            0.574837021890031D,
            0.578453574861326D,
            0.582024343858676D,
            0.585550324547905D,
            0.589032477259595D,
            0.59247172876739D,
            0.595868973951493D,
            0.599225077356329D,
            0.602540874650522D,
            0.605817173996614D,
            0.609054757337295D,
            0.612254381604298D,
            0.61541677985561D,
            0.618542662346152D,
            0.621632717536648D,
            0.624687613045016D,
            0.627707996544254D,
            0.630694496610482D,
            0.633647723524493D,
            0.636568270029909D,
            0.639456712050809D,
            0.642313609371447D,
            0.645139506280505D,
            0.647934932182131D,
            0.650700402175839D,
            0.653436417607212D,
            0.656143466591184D,
            0.658822024509573D,
            0.661472554484411D,
            0.66409550782849D,
            0.66669132447448D,
            0.669260433383837D,
            0.671803252936687D,
            0.674320191303748D,
            0.676811646801301D,
            0.67927802077539D,
            0.681719668800099D,
            0.684136682835364D,
            0.686530027663416D,
            0.688899743088884D,
            0.691246106137019D,
            0.693569597608282D,
            0.695870474098665D,
            0.698149058977493D,
            0.700405668545747D,
            0.702640547844533D,
            0.704854132795649D,
            0.707046650786529D,
            0.709218391982542D,
            0.711369640495095D,
            0.713500612841849D,
            0.715611708948203D,
            0.717703129878111D,
            0.719775137276507D,
            0.721827987565572D,
            0.723861932092928D,
            0.725877160380894D,
            0.727874031330802D,
            0.72985272128892D,
            0.731813462695932D,
            0.73375648358019D,
            0.735682007676512D,
            0.7375902005084D,
            0.739481388811836D,
            0.74135572669864D,
            0.743213421849454D,
            0.745054678185167D,
            0.746879695963207D,
            0.748688619020794D,
            0.750481749253177D,
            0.752259220455991D,
            0.754021219144332D,
            0.755767928604645D,
            0.757499528973561D,
            0.759216197314203D,
            0.760918107690081D,
            0.762605383622277D,
            0.764278291184745D,
            0.765936945533414D,
            0.767581509435767D,
            0.769212142995361D,
            0.770829003713061D,
            0.772432246546423D,
            0.774022023967297D,
            0.775598441410753D,
            0.777161738053551D,
            0.778712012211105D,
            0.78024940696235D,
            0.781774063163651D,
            0.783286119497052D,
            0.78478571251715D,
            0.786272976696649D,
            0.78774804447063D,
            0.78921100498387D,
            0.790662071339156D,
            0.79210132668817D,
            0.793528895739838D,
            0.79494490136878D,
            0.796349464652792D,
            0.797742704909326D,
            0.799124739730996D,
            0.800495685020142D,
            0.803204724311528D,
            0.807187977916127D,
            0.811077371876912D,
            0.814875702113214D,
            0.818585618277199D,
            0.822209768495571D,
            0.825750590359389D,
            0.829210464481729D,
            0.832591681007965D,
            0.835896416633999D,
            0.839126852691535D,
            0.842285003237061D,
            0.845372840018341D,
            0.848392264655648D,
            0.8513450875976D,
            0.854233131455882D,
            0.857058079209698D,
            0.859821583311996D,
            0.862525240823483D,
            0.865170572827866D,
            0.867759121303458D,
            0.870292305538227D,
            0.872771524186588D,
            0.875198131444714D,
            0.877573438891389D,
            0.87989871723085D,
            0.88217517905212D,
            0.884404057825744D,
            0.886586490255283D,
            0.88872359925141D,
            0.890816474300932D,
            0.892866172741099D,
            0.89487372097185D,
            0.896840115609732D,
            0.898766309126233D,
            0.900653274156644D,
            0.902501907342376D,
            0.904313096669019D,
            0.906087705345112D,
            0.907826572671126D,
            0.909530514869862D,
            0.911200325880368D,
            0.91283677811737D,
            0.914440610332822D,
            0.916012580867448D,
            0.91755338774408D,
            0.919063724255905D,
            0.920544265511359D,
            0.921995669021665D,
            0.9234185752646D,
            0.924813608225696D,
            0.926181375917969D,
            0.927522470881238D,
            0.92883747066201D,
            0.930126938274865D,
            0.931391413235491D,
            0.932631450365266D,
            0.933847561473691D,
            0.93504025572627D,
            0.936210029814549D,
            0.937357368323321D,
            0.938482744084522D,
            0.93958661851842D,
            0.940669441962641D,
            0.941731653989576D,
            0.942428556882771D,
            0.944132384665087D,
            0.945782819446051D,
            0.947381675355258D,
            0.948930716975529D,
            0.950431620606918D,
            0.951886006087956D,
            0.953295432173242D,
            0.954661399214426D,
            0.955985351700319D,
            0.957268680665045D,
            0.958512725972489D,
            0.959718778484665D,
            0.960888082121082D,
            0.962021831481095D,
            0.963121191496804D,
            0.964187271784114D,
            0.965221147132101D,
            0.966223854262712D,
            0.967196393358976D,
            0.968139729521551D,
            0.969054794157608D,
            0.969942486305759D,
            0.97080367390052D,
            0.971639194979567D,
            0.972449858836837D,
            0.973236447124328D,
            0.973999714905283D,
            0.974740391661281D,
            0.975459179859994D,
            0.976156765683613D,
            0.976833804843215D,
            0.977490933724627D,
            0.97812876755368D,
            0.978747901171718D,
            0.979348909778888D,
            0.97993234964675D,
            0.980498758801685D,
            0.981048657680479D,
            0.981582549759383D,
            0.982100922157895D,
            0.982604246218422D,
            0.983092978062936D,
            0.983567559127662D,
            0.9840284166768D,
            0.984475964296206D,
            0.984910602367941D,
            0.985332718526515D,
            0.985742686843786D,
            0.986140873370432D,
            0.986527628697813D,
            0.986903293694852D,
            0.987268198516353D,
            0.98762266296499D,
            0.987966996839835D,
            0.988301500271994D,
            0.988626464047885D,
            0.98894216992068D,
            0.989248890910371D,
            0.989546891592956D,
            0.989836428379161D,
            0.990117749783137D,
            0.990391096681522D,
            0.990656702563266D,
            0.990914793770568D,
            0.991165589731292D,
            0.99140930318319D,
            0.991646140390246D,
            0.99187630135146D,
            0.992099980002351D,
            0.99231736440947D,
            0.992528636958178D,
            0.992733974533961D,
            0.992933548697514D,
            0.993127525853832D,
            0.993316067415539D,
            0.993499329960662D,
            0.993677464946003D,
            0.993850620642248D,
            0.994018939544829D,
            0.994182560367352D,
            0.99434161770136D,
            0.994496242144851D,
            0.994646560426529D,
            0.994792695525929D,
            0.99493476678958D,
            0.995072890043338D,
            0.995207177701025D,
            0.995337738869511D,
            0.99546467945036D,
            0.995588102238167D,
            0.995708107015695D,
            0.995824790645932D,
            0.995938247161163D,
            0.996048567849182D,
            0.996155841336717D,
            0.99626015367019D,
            0.99636158839388D,
            0.9964602266256D,
            0.996556147129953D

        };

        public static double ChiTableValueOneDegreeOfFreedom(double paramDblInput)
        {


            int intInput = (Int32)System.Math.Round(paramDblInput * 1000D, 0);
            int intInputModified = 0;

            if (intInput > 8506)
            {
                return .998D;
            }

            if (intInput <= 76)
            {
                return m_objArrayOfDoubleChiDist[intInput];

            }

            if (intInput <= 1626)
            {

                intInputModified = (intInput - 76) / 10;
                return m_objArrayOfDoubleChiDist[76 + intInputModified];

            }

            if (intInput <= 3556)
            {
                intInputModified = (intInput - 1626) / 30;
                return m_objArrayOfDoubleChiDist[76 + ((1627 - 76) / 10) + intInputModified];
            }

            intInputModified = (intInput - 3556) / 50;
            return m_objArrayOfDoubleChiDist[76 + ((1627 - 76) / 10) + ((3557 - 1626) / 30) + intInputModified];


        }
    }
}