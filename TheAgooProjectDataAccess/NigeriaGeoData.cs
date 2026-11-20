using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheAgooProjectDataAccess
{
    /// <summary>
    /// Nigerian states and local government areas used to validate bulk student imports.
    /// The LGA data mirrors wwwroot/js/lga.js so the bulk import accepts the same values as
    /// the single-student admission form.
    /// </summary>
    public static class NigeriaGeoData
    {
        public static readonly IReadOnlyList<string> States = new List<string>
        {
            "Abia",
            "Adamawa",
            "AkwaIbom",
            "Anambra",
            "Bauchi",
            "Bayelsa",
            "Benue",
            "Borno",
            "Cross River",
            "Delta",
            "Ebonyi",
            "Edo",
            "Ekiti",
            "Enugu",
            "FCT",
            "Gombe",
            "Imo",
            "Jigawa",
            "Kaduna",
            "Kano",
            "Katsina",
            "Kebbi",
            "Kogi",
            "Kwara",
            "Lagos",
            "Nasarawa",
            "Niger",
            "Ogun",
            "Ondo",
            "Osun",
            "Oyo",
            "Plateau",
            "Rivers",
            "Sokoto",
            "Taraba",
            "Yobe",
            "Zamfara"
        };

        private static readonly Dictionary<string, string[]> LgaByState = new(StringComparer.OrdinalIgnoreCase)
        {
            {
                "Abia",
                new[]
                {
                    "Aba North",
                    "Aba South",
                    "Arochukwu",
                    "Bende",
                    "Ikwuano",
                    "Isiala Ngwa North",
                    "Isiala Ngwa South",
                    "Isuikwuato",
                    "Obi Ngwa",
                    "Ohafia",
                    "Osisioma",
                    "Ugwunagbo",
                    "Ukwa East",
                    "Ukwa West",
                    "Umuahia North",
                    "Umuahia South",
                    "Umu Nneochi"
                }
            },
            {
                "Adamawa",
                new[]
                {
                    "Demsa",
                    "Fufure",
                    "Ganye",
                    "Gayuk",
                    "Gombi",
                    "Grie",
                    "Hong",
                    "Jada",
                    "Larmurde",
                    "Madagali",
                    "Maiha",
                    "Mayo Belwa",
                    "Michika",
                    "Mubi North",
                    "Mubi South",
                    "Numan",
                    "Shelleng",
                    "Song",
                    "Toungo",
                    "Yola North",
                    "Yola South"
                }
            },
            {
                "AkwaIbom",
                new[]
                {
                    "Abak",
                    "Eastern Obolo",
                    "Eket",
                    "Esit Eket",
                    "Essien Udim",
                    "Etim Ekpo",
                    "Etinan",
                    "Ibeno",
                    "Ibesikpo Asutan",
                    "Ibiono-Ibom",
                    "Ika",
                    "Ikono",
                    "Ikot Abasi",
                    "Ikot Ekpene",
                    "Ini",
                    "Itu",
                    "Mbo",
                    "Mkpat-Enin",
                    "Nsit-Atai",
                    "Nsit-Ibom",
                    "Nsit-Ubium",
                    "Obot Akara",
                    "Okobo",
                    "Onna",
                    "Oron",
                    "Oruk Anam",
                    "Udung-Uko",
                    "Ukanafun",
                    "Uruan",
                    "Urue-Offong Oruko",
                    "Uyo"
                }
            },
            {
                "Anambra",
                new[]
                {
                    "Aguata",
                    "Anambra East",
                    "Anambra West",
                    "Anaocha",
                    "Awka North",
                    "Awka South",
                    "Ayamelum",
                    "Dunukofia",
                    "Ekwusigo",
                    "Idemili North",
                    "Idemili South",
                    "Ihiala",
                    "Njikoka",
                    "Nnewi North",
                    "Nnewi South",
                    "Ogbaru",
                    "Onitsha North",
                    "Onitsha South",
                    "Orumba North",
                    "Orumba South",
                    "Oyi"
                }
            },
            {
                "Bauchi",
                new[]
                {
                    "Alkaleri",
                    "Bauchi",
                    "Bogoro",
                    "Damban",
                    "Darazo",
                    "Dass",
                    "Gamawa",
                    "Ganjuwa",
                    "Giade",
                    "Itas-Gadau",
                    "Jama are",
                    "Katagum",
                    "Kirfi",
                    "Misau",
                    "Ningi",
                    "Shira",
                    "Tafawa Balewa",
                    "Toro",
                    "Warji",
                    "Zaki",
                    "Jama'are"
                }
            },
            {
                "Bayelsa",
                new[]
                {
                    "Brass",
                    "Ekeremor",
                    "Kolokuma Opokuma",
                    "Nembe",
                    "Ogbia",
                    "Sagbama",
                    "Southern Ijaw",
                    "Yenagoa"
                }
            },
            {
                "Benue",
                new[]
                {
                    "Agatu",
                    "Apa",
                    "Ado",
                    "Buruku",
                    "Gboko",
                    "Guma",
                    "Gwer East",
                    "Gwer West",
                    "Katsina-Ala",
                    "Konshisha",
                    "Kwande",
                    "Logo",
                    "Makurdi",
                    "Obi",
                    "Ogbadibo",
                    "Ohimini",
                    "Oju",
                    "Okpokwu",
                    "Oturkpo",
                    "Tarka",
                    "Ukum",
                    "Ushongo",
                    "Vandeikya"
                }
            },
            {
                "Borno",
                new[]
                {
                    "Abadam",
                    "Askira-Uba",
                    "Bama",
                    "Bayo",
                    "Biu",
                    "Chibok",
                    "Damboa",
                    "Dikwa",
                    "Gubio",
                    "Guzamala",
                    "Gwoza",
                    "Hawul",
                    "Jere",
                    "Kaga",
                    "Kala-Balge",
                    "Konduga",
                    "Kukawa",
                    "Kwaya Kusar",
                    "Mafa",
                    "Magumeri",
                    "Maiduguri",
                    "Marte",
                    "Mobbar",
                    "Monguno",
                    "Ngala",
                    "Nganzai",
                    "Shani"
                }
            },
            {
                "Cross River",
                new[]
                {
                    "Abi",
                    "Akamkpa",
                    "Akpabuyo",
                    "Bakassi",
                    "Bekwarra",
                    "Biase",
                    "Boki",
                    "Calabar Municipal",
                    "Calabar South",
                    "Etung",
                    "Ikom",
                    "Obanliku",
                    "Obubra",
                    "Obudu",
                    "Odukpani",
                    "Ogoja",
                    "Yakuur",
                    "Yala"
                }
            },
            {
                "Delta",
                new[]
                {
                    "Aniocha North",
                    "Aniocha South",
                    "Bomadi",
                    "Burutu",
                    "Ethiope East",
                    "Ethiope West",
                    "Ika North East",
                    "Ika South",
                    "Isoko North",
                    "Isoko South",
                    "Ndokwa East",
                    "Ndokwa West",
                    "Okpe",
                    "Oshimili North",
                    "Oshimili South",
                    "Patani",
                    "Sapele",
                    "Udu",
                    "Ughelli North",
                    "Ughelli South",
                    "Ukwuani",
                    "Uvwie",
                    "Warri North",
                    "Warri South",
                    "Warri South West"
                }
            },
            {
                "Ebonyi",
                new[]
                {
                    "Abakaliki",
                    "Afikpo North",
                    "Afikpo South",
                    "Ebonyi",
                    "Ezza North",
                    "Ezza South",
                    "Ikwo",
                    "Ishielu",
                    "Ivo",
                    "Izzi",
                    "Ohaozara",
                    "Ohaukwu",
                    "Onicha"
                }
            },
            {
                "Edo",
                new[]
                {
                    "Akoko-Edo",
                    "Egor",
                    "Esan Central",
                    "Esan North-East",
                    "Esan South-East",
                    "Esan West",
                    "Etsako Central",
                    "Etsako East",
                    "Etsako West",
                    "Igueben",
                    "Ikpoba Okha",
                    "Orhionmwon",
                    "Oredo",
                    "Ovia North-East",
                    "Ovia South-West",
                    "Owan East",
                    "Owan West",
                    "Uhunmwonde"
                }
            },
            {
                "Ekiti",
                new[]
                {
                    "Ado Ekiti",
                    "Efon",
                    "Ekiti East",
                    "Ekiti South-West",
                    "Ekiti West",
                    "Emure",
                    "Gbonyin",
                    "Ido Osi",
                    "Ijero",
                    "Ikere",
                    "Ikole",
                    "Ilejemeje",
                    "Irepodun-Ifelodun",
                    "Ise-Orun",
                    "Moba",
                    "Oye"
                }
            },
            {
                "Enugu",
                new[]
                {
                    "Aninri",
                    "Awgu",
                    "Enugu East",
                    "Enugu North",
                    "Enugu South",
                    "Ezeagu",
                    "Igbo Etiti",
                    "Igbo Eze North",
                    "Igbo Eze South",
                    "Isi Uzo",
                    "Nkanu East",
                    "Nkanu West",
                    "Nsukka",
                    "Oji River",
                    "Udenu",
                    "Udi",
                    "Uzo Uwani"
                }
            },
            {
                "FCT",
                new[]
                {
                    "Abaji",
                    "Bwari",
                    "Gwagwalada",
                    "Kuje",
                    "Kwali",
                    "Municipal Area Council"
                }
            },
            {
                "Gombe",
                new[]
                {
                    "Akko",
                    "Balanga",
                    "Billiri",
                    "Dukku",
                    "Funakaye",
                    "Gombe",
                    "Kaltungo",
                    "Kwami",
                    "Nafada",
                    "Shongom",
                    "Yamaltu-Deba"
                }
            },
            {
                "Imo",
                new[]
                {
                    "Aboh Mbaise",
                    "Ahiazu Mbaise",
                    "Ehime Mbano",
                    "Ezinihitte",
                    "Ideato North",
                    "Ideato South",
                    "Ihitte-Uboma",
                    "Ikeduru",
                    "Isiala Mbano",
                    "Isu",
                    "Mbaitoli",
                    "Ngor Okpala",
                    "Njaba",
                    "Nkwerre",
                    "Nwangele",
                    "Obowo",
                    "Oguta",
                    "Ohaji-Egbema",
                    "Okigwe",
                    "Orlu",
                    "Orsu",
                    "Oru East",
                    "Oru West",
                    "Owerri Municipal",
                    "Owerri North",
                    "Owerri West",
                    "Unuimo"
                }
            },
            {
                "Jigawa",
                new[]
                {
                    "Auyo",
                    "Babura",
                    "Biriniwa",
                    "Birnin Kudu",
                    "Buji",
                    "Dutse",
                    "Gagarawa",
                    "Garki",
                    "Gumel",
                    "Guri",
                    "Gwaram",
                    "Gwiwa",
                    "Hadejia",
                    "Jahun",
                    "Kafin Hausa",
                    "Kazaure",
                    "Kiri Kasama",
                    "Kiyawa",
                    "Kaugama",
                    "Maigatari",
                    "Malam Madori",
                    "Miga",
                    "Ringim",
                    "Roni",
                    "Sule Tankarkar",
                    "Taura",
                    "Yankwashi"
                }
            },
            {
                "Kaduna",
                new[]
                {
                    "Birnin Gwari",
                    "Chikun",
                    "Giwa",
                    "Igabi",
                    "Ikara",
                    "Jaba",
                    "Jema a",
                    "Kachia",
                    "Kaduna North",
                    "Kaduna South",
                    "Kagarko",
                    "Kajuru",
                    "Kaura",
                    "Kauru",
                    "Kubau",
                    "Kudan",
                    "Lere",
                    "Makarfi",
                    "Sabon Gari",
                    "Sanga",
                    "Soba",
                    "Zangon Kataf",
                    "Zaria"
                }
            },
            {
                "Kano",
                new[]
                {
                    "Ajingi",
                    "Albasu",
                    "Bagwai",
                    "Bebeji",
                    "Bichi",
                    "Bunkure",
                    "Dala",
                    "Dambatta",
                    "Dawakin Kudu",
                    "Dawakin Tofa",
                    "Doguwa",
                    "Fagge",
                    "Gabasawa",
                    "Garko",
                    "Garun Mallam",
                    "Gaya",
                    "Gezawa",
                    "Gwale",
                    "Gwarzo",
                    "Kabo",
                    "Kano Municipal",
                    "Karaye",
                    "Kibiya",
                    "Kiru",
                    "Kumbotso",
                    "Kunchi",
                    "Kura",
                    "Madobi",
                    "Makoda",
                    "Minjibir",
                    "Nasarawa",
                    "Rano",
                    "Rimin Gado",
                    "Rogo",
                    "Shanono",
                    "Sumaila",
                    "Takai",
                    "Tarauni",
                    "Tofa",
                    "Tsanyawa",
                    "Tudun Wada",
                    "Ungogo",
                    "Warawa",
                    "Wudil"
                }
            },
            {
                "Katsina",
                new[]
                {
                    "Bakori",
                    "Batagarawa",
                    "Batsari",
                    "Baure",
                    "Bindawa",
                    "Charanchi",
                    "Dandume",
                    "Danja",
                    "Dan Musa",
                    "Daura",
                    "Dutsi",
                    "Dutsin Ma",
                    "Faskari",
                    "Funtua",
                    "Ingawa",
                    "Jibia",
                    "Kafur",
                    "Kaita",
                    "Kankara",
                    "Kankia",
                    "Katsina",
                    "Kurfi",
                    "Kusada",
                    "Mai Adua",
                    "Malumfashi",
                    "Mani",
                    "Mashi",
                    "Matazu",
                    "Musawa",
                    "Rimi",
                    "Sabuwa",
                    "Safana",
                    "Sandamu",
                    "Zango"
                }
            },
            {
                "Kebbi",
                new[]
                {
                    "Aleiro",
                    "Arewa Dandi",
                    "Argungu",
                    "Augie",
                    "Bagudo",
                    "Birnin Kebbi",
                    "Bunza",
                    "Dandi",
                    "Fakai",
                    "Gwandu",
                    "Jega",
                    "Kalgo",
                    "Koko Besse",
                    "Maiyama",
                    "Ngaski",
                    "Sakaba",
                    "Shanga",
                    "Suru",
                    "Wasagu Danko",
                    "Yauri",
                    "Zuru"
                }
            },
            {
                "Kogi",
                new[]
                {
                    "Adavi",
                    "Ajaokuta",
                    "Ankpa",
                    "Bassa",
                    "Dekina",
                    "Ibaji",
                    "Idah",
                    "Igalamela Odolu",
                    "Ijumu",
                    "Kabba Bunu",
                    "Kogi",
                    "Lokoja",
                    "Mopa Muro",
                    "Ofu",
                    "Ogori Magongo",
                    "Okehi",
                    "Okene",
                    "Olamaboro",
                    "Omala",
                    "Yagba East",
                    "Yagba West"
                }
            },
            {
                "Kwara",
                new[]
                {
                    "Asa",
                    "Baruten",
                    "Edu",
                    "Ekiti",
                    "Ifelodun",
                    "Ilorin East",
                    "Ilorin South",
                    "Ilorin West",
                    "Irepodun",
                    "Isin",
                    "Kaiama",
                    "Moro",
                    "Offa",
                    "Oke Ero",
                    "Oyun",
                    "Pategi"
                }
            },
            {
                "Lagos",
                new[]
                {
                    "Agege",
                    "Ajeromi-Ifelodun",
                    "Alimosho",
                    "Amuwo-Odofin",
                    "Apapa",
                    "Badagry",
                    "Epe",
                    "Eti Osa",
                    "Ibeju-Lekki",
                    "Ifako-Ijaiye",
                    "Ikeja",
                    "Ikorodu",
                    "Kosofe",
                    "Lagos Island",
                    "Lagos Mainland",
                    "Mushin",
                    "Ojo",
                    "Oshodi-Isolo",
                    "Shomolu",
                    "Surulere"
                }
            },
            {
                "Nasarawa",
                new[]
                {
                    "Akwanga",
                    "Awe",
                    "Doma",
                    "Karu",
                    "Keana",
                    "Keffi",
                    "Kokona",
                    "Lafia",
                    "Nasarawa",
                    "Nasarawa Egon",
                    "Obi",
                    "Toto",
                    "Wamba"
                }
            },
            {
                "Niger",
                new[]
                {
                    "Agaie",
                    "Agwara",
                    "Bida",
                    "Borgu",
                    "Bosso",
                    "Chanchaga",
                    "Edati",
                    "Gbako",
                    "Gurara",
                    "Katcha",
                    "Kontagora",
                    "Lapai",
                    "Lavun",
                    "Magama",
                    "Mariga",
                    "Mashegu",
                    "Mokwa",
                    "Moya",
                    "Paikoro",
                    "Rafi",
                    "Rijau",
                    "Shiroro",
                    "Suleja",
                    "Tafa",
                    "Wushishi"
                }
            },
            {
                "Ogun",
                new[]
                {
                    "Abeokuta North",
                    "Abeokuta South",
                    "Ado-Odo Ota",
                    "Egbado North",
                    "Egbado South",
                    "Ewekoro",
                    "Ifo",
                    "Ijebu East",
                    "Ijebu North",
                    "Ijebu North East",
                    "Ijebu Ode",
                    "Ikenne",
                    "Imeko Afon",
                    "Ipokia",
                    "Obafemi Owode",
                    "Odeda",
                    "Odogbolu",
                    "Ogun Waterside",
                    "Remo North",
                    "Shagamu"
                }
            },
            {
                "Ondo",
                new[]
                {
                    "Akoko North-East",
                    "Akoko North-West",
                    "Akoko South-West",
                    "Akoko South-East",
                    "Akure North",
                    "Akure South",
                    "Ese Odo",
                    "Idanre",
                    "Ifedore",
                    "Ilaje",
                    "Ile Oluji-Okeigbo",
                    "Irele",
                    "Odigbo",
                    "Okitipupa",
                    "Ondo East",
                    "Ondo West",
                    "Ose",
                    "Owo"
                }
            },
            {
                "Osun",
                new[]
                {
                    "Atakunmosa East",
                    "Atakunmosa West",
                    "Aiyedaade",
                    "Aiyedire",
                    "Boluwaduro",
                    "Boripe",
                    "Ede North",
                    "Ede South",
                    "Ife Central",
                    "Ife East",
                    "Ife North",
                    "Ife South",
                    "Egbedore",
                    "Ejigbo",
                    "Ifedayo",
                    "Ifelodun",
                    "Ila",
                    "Ilesa East",
                    "Ilesa West",
                    "Irepodun",
                    "Irewole",
                    "Isokan",
                    "Iwo",
                    "Obokun",
                    "Odo Otin",
                    "Ola Oluwa",
                    "Olorunda",
                    "Oriade",
                    "Orolu",
                    "Osogbo"
                }
            },
            {
                "Oyo",
                new[]
                {
                    "Afijio",
                    "Akinyele",
                    "Atiba",
                    "Atisbo",
                    "Egbeda",
                    "Ibadan North",
                    "Ibadan North-East",
                    "Ibadan North-West",
                    "Ibadan South-East",
                    "Ibadan South-West",
                    "Ibarapa Central",
                    "Ibarapa East",
                    "Ibarapa North",
                    "Ido",
                    "Irepo",
                    "Iseyin",
                    "Itesiwaju",
                    "Iwajowa",
                    "Kajola",
                    "Lagelu",
                    "Ogbomosho North",
                    "Ogbomosho South",
                    "Ogo Oluwa",
                    "Olorunsogo",
                    "Oluyole",
                    "Ona Ara",
                    "Orelope",
                    "Ori Ire",
                    "Oyo",
                    "Oyo East",
                    "Saki East",
                    "Saki West",
                    "Surulere"
                }
            },
            {
                "Plateau",
                new[]
                {
                    "Bokkos",
                    "Barkin Ladi",
                    "Bassa",
                    "Jos East",
                    "Jos North",
                    "Jos South",
                    "Kanam",
                    "Kanke",
                    "Langtang South",
                    "Langtang North",
                    "Mangu",
                    "Mikang",
                    "Pankshin",
                    "Qua an Pan",
                    "Riyom",
                    "Shendam",
                    "Wase"
                }
            },
            {
                "Rivers",
                new[]
                {
                    "Port Harcourt",
                    "Obio-Akpor",
                    "Okrika",
                    "Ogu–Bolo",
                    "Eleme",
                    "Tai",
                    "Gokana",
                    "Khana",
                    "Oyigbo",
                    "Opobo–Nkoro",
                    "Andoni",
                    "Bonny",
                    "Degema",
                    "Asari-Toru",
                    "Akuku-Toru",
                    "Abua–Odual",
                    "Ahoada West",
                    "Ahoada East",
                    "Ogba–Egbema–Ndoni",
                    "Emohua",
                    "Ikwerre",
                    "Etche",
                    "Omuma"
                }
            },
            {
                "Sokoto",
                new[]
                {
                    "Binji",
                    "Bodinga",
                    "Dange Shuni",
                    "Gada",
                    "Goronyo",
                    "Gudu",
                    "Gwadabawa",
                    "Illela",
                    "Isa",
                    "Kebbe",
                    "Kware",
                    "Rabah",
                    "Sabon Birni",
                    "Shagari",
                    "Silame",
                    "Sokoto North",
                    "Sokoto South",
                    "Tambuwal",
                    "Tangaza",
                    "Tureta",
                    "Wamako",
                    "Wurno",
                    "Yabo"
                }
            },
            {
                "Taraba",
                new[]
                {
                    "Ardo Kola",
                    "Bali",
                    "Donga",
                    "Gashaka",
                    "Gassol",
                    "Ibi",
                    "Jalingo",
                    "Karim Lamido",
                    "Kumi",
                    "Lau",
                    "Sardauna",
                    "Takum",
                    "Ussa",
                    "Wukari",
                    "Yorro",
                    "Zing"
                }
            },
            {
                "Yobe",
                new[]
                {
                    "Bade",
                    "Bursari",
                    "Damaturu",
                    "Fika",
                    "Fune",
                    "Geidam",
                    "Gujba",
                    "Gulani",
                    "Jakusko",
                    "Karasuwa",
                    "Machina",
                    "Nangere",
                    "Nguru",
                    "Potiskum",
                    "Tarmuwa",
                    "Yunusari",
                    "Yusufari"
                }
            },
            {
                "Zamfara",
                new[]
                {
                    "Anka",
                    "Bakura",
                    "Birnin Magaji Kiyaw",
                    "Bukkuyum",
                    "Bungudu",
                    "Gummi",
                    "Gusau",
                    "Kaura Namoda",
                    "Maradun",
                    "Maru",
                    "Shinkafi",
                    "Talata Mafara",
                    "Chafe",
                    "Zurmi",
                    "Tsafe"
                }
            }
        };

        private static readonly Dictionary<string, string> StateAliases = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Abuja", "FCT" },
            { "Akwa Ibom", "AkwaIbom" },
            { "Federal Capital Territory", "FCT" },
            { "Zamafara", "Zamfara" }
        };

        /// <summary>
        /// Resolves a trimmed, case-insensitive state name to its canonical spelling.
        /// </summary>
        public static bool TryGetState(string? value, out string state)
        {
            state = string.Empty;
            var key = NormalizeKey(value);
            if (key.Length == 0)
            {
                return false;
            }

            foreach (var candidate in States)
            {
                if (NormalizeKey(candidate) == key)
                {
                    state = candidate;
                    return true;
                }
            }

            foreach (var alias in StateAliases)
            {
                if (NormalizeKey(alias.Key) == key)
                {
                    state = alias.Value;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the closest state spelling for a mistyped value, or null when no close match exists.
        /// </summary>
        public static string? SuggestState(string? value)
        {
            return Suggest(value, States);
        }

        /// <summary>
        /// Resolves a trimmed, case-insensitive LGA name to its canonical spelling for the supplied state.
        /// </summary>
        public static bool TryGetLocalGov(string state, string? value, out string localGov)
        {
            localGov = string.Empty;
            var key = NormalizeKey(value);
            if (key.Length == 0 || !LgaByState.TryGetValue(state, out var lgas))
            {
                return false;
            }

            foreach (var candidate in lgas)
            {
                if (NormalizeKey(candidate) == key)
                {
                    localGov = candidate;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the closest LGA spelling for a mistyped value, or null when no close match exists.
        /// </summary>
        public static string? SuggestLocalGov(string state, string? value)
        {
            if (string.IsNullOrWhiteSpace(value) || !LgaByState.TryGetValue(state, out var lgas))
            {
                return null;
            }

            return Suggest(value, lgas);
        }

        private static string? Suggest(string? value, IEnumerable<string> candidates)
        {
            var key = NormalizeKey(value);
            if (key.Length == 0)
            {
                return null;
            }

            string? best = null;
            var bestDistance = int.MaxValue;
            foreach (var candidate in candidates)
            {
                var distance = Levenshtein(key, NormalizeKey(candidate));
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    best = candidate;
                }
            }

            var maxDistance = key.Length <= 4 ? 1 : key.Length <= 8 ? 2 : 3;
            return bestDistance <= maxDistance ? best : null;
        }

        private static string NormalizeKey(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var builder = new StringBuilder(value.Length);
            foreach (var character in value.Trim().ToLowerInvariant())
            {
                builder.Append(char.IsLetterOrDigit(character) ? character : ' ');
            }

            return string.Join(" ", builder.ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries));
        }

        private static int Levenshtein(string source, string target)
        {
            if (source == target)
            {
                return 0;
            }

            if (source.Length == 0)
            {
                return target.Length;
            }

            if (target.Length == 0)
            {
                return source.Length;
            }

            var previous = new int[target.Length + 1];
            var current = new int[target.Length + 1];
            for (var j = 0; j <= target.Length; j++)
            {
                previous[j] = j;
            }

            for (var i = 1; i <= source.Length; i++)
            {
                current[0] = i;
                for (var j = 1; j <= target.Length; j++)
                {
                    var cost = source[i - 1] == target[j - 1] ? 0 : 1;
                    current[j] = Math.Min(Math.Min(current[j - 1] + 1, previous[j] + 1), previous[j - 1] + cost);
                }

                (previous, current) = (current, previous);
            }

            return previous[target.Length];
        }
    }
}
