using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI_DotNetCore_Demo.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    ISOCode = table.Column<string>(maxLength: 2, nullable: false),
                    CountryCode = table.Column<string>(maxLength: 5, nullable: false),
                    SortOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Gender",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 3, nullable: false),
                    Name = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gender", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 100, nullable: false),
                    LastName = table.Column<string>(maxLength: 100, nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: true),
                    GenderID = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Person_Gender_GenderID",
                        column: x => x.GenderID,
                        principalTable: "Gender",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    AddressType = table.Column<string>(maxLength: 20, nullable: true),
                    FirstLine = table.Column<string>(maxLength: 100, nullable: false),
                    SecondLine = table.Column<string>(maxLength: 100, nullable: true),
                    ThirdLine = table.Column<string>(maxLength: 100, nullable: true),
                    PostCode = table.Column<string>(maxLength: 20, nullable: false),
                    City = table.Column<string>(maxLength: 100, nullable: false),
                    State = table.Column<string>(maxLength: 100, nullable: false),
                    CountryID = table.Column<Guid>(nullable: false),
                    PersonID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Addresses_Countries",
                        column: x => x.CountryID,
                        principalTable: "Country",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Addresses_Persons",
                        column: x => x.PersonID,
                        principalTable: "Person",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhoneNumber",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    PhoneNumberType = table.Column<string>(maxLength: 20, nullable: true),
                    Number = table.Column<string>(maxLength: 20, nullable: false),
                    CountryID = table.Column<Guid>(nullable: true),
                    PersonID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneNumber", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PhoneNumbers_Countries",
                        column: x => x.CountryID,
                        principalTable: "Country",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhoneNumbers_Persons",
                        column: x => x.PersonID,
                        principalTable: "Person",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "ID", "CountryCode", "ISOCode", "Name", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("ed7d6a7f-db90-42a1-8957-5fa62a1ecc27"), "+60", "MY", "Malaysia", 1 },
                    { new Guid("a8f6838c-aaed-4ee0-8720-34903107b606"), "+687", "NC", "New Caledonia", 155 },
                    { new Guid("f3b3a48d-a5f2-467b-b1f0-934da85a0c68"), "+64", "NZ", "New Zealand", 156 },
                    { new Guid("325fc358-5f64-4680-a6bc-7af5c303fe1b"), "+505", "NI", "Nicaragua", 157 },
                    { new Guid("60beeb4a-bed4-471c-a8ba-f22d2aa7557f"), "+227", "NE", "Niger", 158 },
                    { new Guid("cced640d-d3a6-4778-bc3e-3ea32147d590"), "+234", "NG", "Nigeria", 159 },
                    { new Guid("05ee3eca-a7bd-45c0-b2ba-8f461e241a9c"), "+683", "NU", "Niue", 160 },
                    { new Guid("69e17a7e-ee2d-45a8-908a-fd02fb1c851d"), "+672", "NF", "Norfolk Island", 161 },
                    { new Guid("5b6e3e8d-665b-4f2e-8da2-e47dea62ba4b"), "+1670", "MP", "Northern Mariana Islands", 162 },
                    { new Guid("1bfc6eca-c6b6-476c-8a2f-90d0eccf9e19"), "+47", "NO", "Norway", 163 },
                    { new Guid("d981eb8e-2a46-4314-88c8-27583994ce43"), "+968", "OM", "Oman", 164 },
                    { new Guid("4f2e2f57-187d-4f0f-91b4-86b2168dd6dd"), "+92", "PK", "Pakistan", 165 },
                    { new Guid("e3722d08-1f63-4690-bbd5-d509497ddbef"), "+680", "PW", "Palau", 166 },
                    { new Guid("d6fffd7f-a626-4c3c-a930-56a4c13bfa1d"), "+31", "NL", "Netherlands", 154 },
                    { new Guid("bb32d857-c59f-4b13-9ad7-e99594e7f82e"), "+970", "PS", "Palestine, State of", 167 },
                    { new Guid("d4be2ef3-c3b4-437c-adb2-f4046c7b68ad"), "+675", "PG", "Papua New Guinea", 169 },
                    { new Guid("c857cfaf-d219-427f-8800-6dc2de417293"), "+595", "PY", "Paraguay", 170 },
                    { new Guid("9176a788-9a69-449c-90e5-71d306ec6107"), "+51", "PE", "Peru", 171 },
                    { new Guid("c6341c51-e565-454e-8cf8-16d71044cae6"), "+64", "PN", "Pitcairn", 172 },
                    { new Guid("fcb680a8-7893-47f6-8547-de33c1e64dd9"), "+48", "PL", "Poland", 173 },
                    { new Guid("94370043-fac0-45ac-8db0-c77c6887772e"), "+351", "PT", "Portugal", 174 },
                    { new Guid("806c074d-84df-4cd9-907b-c783ab037705"), "+1787", "PR", "Puerto Rico", 175 },
                    { new Guid("1b5714a3-7c0e-4aed-b324-66778d63796d"), "+974", "QA", "Qatar", 176 },
                    { new Guid("92290c3b-f2b2-4cce-80d2-711189bea730"), "+262", "RE", "Réunion", 177 },
                    { new Guid("14f47df7-6e6d-47c4-a9f3-433400426217"), "+40", "RO", "Romania", 178 },
                    { new Guid("29a4b7dc-46e9-4a51-a8b5-d12fabe9baed"), "+7", "RU", "Russian Federation", 179 },
                    { new Guid("5141ae66-7f65-40a8-8f26-f0f5480f1683"), "+250", "RW", "Rwanda", 180 },
                    { new Guid("ac018dd3-599f-48db-ad25-f54a434832af"), "+507", "PA", "Panama", 168 },
                    { new Guid("27a1735b-a974-4703-b920-a87c0b58c7a3"), "+590", "BL", "Saint Barthélemy", 181 },
                    { new Guid("e38d6df4-9316-4cb8-8d6c-d5f58889fb40"), "+977", "NP", "Nepal", 153 },
                    { new Guid("ba68baec-705e-4baa-847c-ce80fb24c53d"), "+264", "NA", "Namibia", 151 },
                    { new Guid("09bc800e-cbf9-49f9-9da0-dd93e2078ad2"), "+231", "LR", "Liberia", 125 },
                    { new Guid("0bd86c65-f09e-4833-b518-f98e45f4e93c"), "+218", "LY", "Libya", 126 },
                    { new Guid("34978708-5249-4566-92a7-8c1dfff87139"), "+423", "LI", "Liechtenstein", 127 },
                    { new Guid("b2e89198-44bc-4943-87ca-f0e0aedf6736"), "+370", "LT", "Lithuania", 128 },
                    { new Guid("00487e1d-6d06-416c-9017-9e61eda4855f"), "+352", "LU", "Luxembourg", 129 },
                    { new Guid("2b5e1ce3-e581-4990-82e3-61d7e92f4be7"), "+853", "MO", "Macao", 130 },
                    { new Guid("bc36da38-a035-4ca4-adb1-d45b6ff67fdb"), "+389", "MK", "Macedonia, the Former Yugoslav Republic of", 131 },
                    { new Guid("d5237f5e-3596-4b77-9611-fa6d22879520"), "+261", "MG", "Madagascar", 132 },
                    { new Guid("afd9983a-fcf2-4a3a-bdf5-7f91a8aad302"), "+265", "MW", "Malawi", 133 },
                    { new Guid("680a9e5b-7023-449c-ad13-3eca405898c3"), "+960", "MV", "Maldives", 134 },
                    { new Guid("c5f20036-5ed6-4ddf-8e8f-eace56f1b3bc"), "+223", "ML", "Mali", 135 },
                    { new Guid("4f187427-484f-4802-b5e5-8aa5666c6d45"), "+356", "MT", "Malta", 136 },
                    { new Guid("a62a85ad-9da1-4032-b2f9-42433d446189"), "+674", "NR", "Nauru", 152 },
                    { new Guid("fd25efb7-df2d-4b42-819a-843035ac4043"), "+692", "MH", "Marshall Islands", 137 },
                    { new Guid("1bcf007d-f531-4835-8255-ce1a597d8673"), "+222", "MR", "Mauritania", 139 },
                    { new Guid("6fbf6b57-96ab-439a-b73b-56c2ccda91c7"), "+230", "MU", "Mauritius", 140 },
                    { new Guid("2e7357a1-95af-46b4-ad41-78d6af1920d4"), "+262", "YT", "Mayotte", 141 },
                    { new Guid("23689be6-4731-4b53-99f0-69a0be188ada"), "+52", "MX", "Mexico", 142 },
                    { new Guid("4370f31d-c8fc-4634-a760-9457e311765a"), "+691", "FM", "Micronesia, Federated States of", 143 },
                    { new Guid("46fe46f9-ea5f-4382-bb84-2e12bc4299c8"), "+373", "MD", "Moldova, Republic of", 144 },
                    { new Guid("05e3e28b-31e1-4dc8-839a-20584fa98e2c"), "+377", "MC", "Monaco", 145 },
                    { new Guid("cea58c0b-b4b8-4f78-b15f-04aa5941a693"), "+976", "MN", "Mongolia", 146 },
                    { new Guid("f3a5ee45-dd49-4eb4-a46c-c07a6534ccaa"), "+382", "ME", "Montenegro", 147 },
                    { new Guid("a6369824-21da-40be-86b5-55c9a9752027"), "+1664", "MS", "Montserrat", 148 },
                    { new Guid("06cc3fc5-e010-4060-a2f1-25ae710d99f2"), "+212", "MA", "Morocco", 149 },
                    { new Guid("26b99f45-2e5f-45ca-bdab-b74b1d5e5990"), "+258", "MZ", "Mozambique", 150 },
                    { new Guid("c1de9e5a-7307-4d84-8cff-d242e9e5eaff"), "+596", "MQ", "Martinique", 138 },
                    { new Guid("2dec353c-3fcc-494d-99d3-9b03ab3a7542"), "+290", "SH", "Saint Helena, Ascension and Tristan da Cunha", 182 },
                    { new Guid("cc9b12f2-5793-4a5d-82ba-537033d860a6"), "+1869", "KN", "Saint Kitts and Nevis", 183 },
                    { new Guid("b6034666-2dd6-45b6-a082-f518bf7c7b44"), "+1758", "LC", "Saint Lucia", 184 },
                    { new Guid("dfc89113-7f8d-4e62-9dcf-fff61cbc4c10"), "+670", "TL", "Timor-Leste", 216 },
                    { new Guid("795b7890-4b95-4025-9ce3-79769cbc1e12"), "+228", "TG", "Togo", 217 },
                    { new Guid("13acfc31-196c-47a3-a0f5-faa79de4822c"), "+690", "TK", "Tokelau", 218 },
                    { new Guid("35f2a72b-7429-4ff2-b5bd-729ff0ab8997"), "+676", "TO", "Tonga", 219 },
                    { new Guid("74c0485d-b20b-4d4b-8577-9fbf18ba73f6"), "+1868", "TT", "Trinidad and Tobago", 220 },
                    { new Guid("b0092114-6220-4559-9541-4fd095d7a996"), "+216", "TN", "Tunisia", 221 },
                    { new Guid("edfbb222-7eed-4cd0-aa48-b02a8e412164"), "+90", "TR", "Turkey", 222 },
                    { new Guid("1008e44c-99b4-465d-81ac-74d9345809e9"), "+993", "TM", "Turkmenistan", 223 },
                    { new Guid("31099c09-f29e-4fe2-9f4b-99e392d26bbf"), "+1649", "TC", "Turks and Caicos Islands", 224 },
                    { new Guid("625fa781-5678-4d8f-8e57-0f4b31b644f6"), "+688", "TV", "Tuvalu", 225 },
                    { new Guid("04f5348e-1142-4e47-81dc-9b34e9e204d6"), "+256", "UG", "Uganda", 226 },
                    { new Guid("68d81b3f-0e4a-463e-a381-cbb81b0f7a08"), "+380", "UA", "Ukraine", 227 },
                    { new Guid("9bc1ecb3-879c-4ef8-ad23-86703aaf23db"), "+255", "TZ", "Tanzania, United Republic of", 215 },
                    { new Guid("8ad4b4c3-b121-4698-98e8-c06342e4e396"), "+971", "AE", "United Arab Emirates", 228 },
                    { new Guid("7291a7af-5a82-4685-ae05-afdeec25a228"), "+1", "US", "United States of America", 230 },
                    { new Guid("50bb9149-e579-49c1-a1de-aecb7ada2aed"), "+598", "UY", "Uruguay", 231 },
                    { new Guid("60c11672-6b3a-41df-b3e9-9cb59dbc5a2b"), "+998", "UZ", "Uzbekistan", 232 },
                    { new Guid("e240f427-e33a-4611-80f6-fa471a7bf1b2"), "+678", "VU", "Vanuatu", 233 },
                    { new Guid("56f6632f-ceff-4978-9473-434f71972deb"), "+58", "VE", "Venezuela, Bolivarian Republic of", 234 },
                    { new Guid("026df4d1-b12c-494f-9de6-cf2bae7ab767"), "+1284", "VG", "Virgin Islands, British", 235 },
                    { new Guid("3b168572-c1c9-44e9-9ee3-a7082b2c0cb3"), "+1340", "VI", "Virgin Islands, U.S.", 236 },
                    { new Guid("c8e5bcfa-3676-49e0-a082-bd9d9281fe3b"), "+681", "WF", "Wallis and Futuna", 237 },
                    { new Guid("9262aa78-f030-4960-88c7-0e6f5c01d495"), "+212", "EH", "Western Sahara", 238 },
                    { new Guid("a1df241c-98bd-4530-af50-7c27a5a98d94"), "+967", "YE", "Yemen", 239 },
                    { new Guid("de167dbf-41e8-4a6b-9ea7-1a3cad6f21f8"), "+260", "ZM", "Zambia", 240 },
                    { new Guid("46185b8c-886e-4854-8d51-fdd835f53393"), "+263", "ZW", "Zimbabwe", 241 },
                    { new Guid("c4c12a58-27e8-49e8-8f83-63a970b87cdf"), "+44", "GB", "United Kingdom", 229 },
                    { new Guid("e63fc86c-e6d1-4912-a8be-057fb982d436"), "+992", "TJ", "Tajikistan", 214 },
                    { new Guid("16bacb85-d512-40de-a22d-87054c3f2267"), "+886", "TW", "Taiwan, Province of China", 213 },
                    { new Guid("d84ea433-dc8b-4d39-85d7-752c4456ca77"), "+963", "SY", "Syrian Arab Republic", 212 },
                    { new Guid("51628537-30b0-4c21-89b1-31281e7bf353"), "+590", "MF", "Saint Martin (French part)", 185 },
                    { new Guid("564f65cc-a50b-4c40-8001-903bc4d966e7"), "+508", "PM", "Saint Pierre and Miquelon", 186 },
                    { new Guid("2714e50d-df39-4f22-a36f-f46bc8528bc5"), "+1784", "VC", "Saint Vincent and the Grenadines", 187 },
                    { new Guid("eeadad9c-ee52-4aef-9d66-e3b6e05ddff0"), "+1684", "AS", "Samoa", 188 },
                    { new Guid("a0d7e957-2d59-4f55-814d-ea12090177b9"), "+378", "SM", "San Marino", 189 },
                    { new Guid("e1014c0b-2290-411c-8f09-cc2ea7efff76"), "+239", "ST", "Sao Tome and Principe", 190 },
                    { new Guid("72772168-5906-4b77-8be3-f2b90f150ff7"), "+966", "SA", "Saudi Arabia", 191 },
                    { new Guid("2de3ab6c-d0a0-4c32-89d7-41968b73fa68"), "+221", "SN", "Senegal", 192 },
                    { new Guid("a7b47d7d-1c4f-4db7-add9-4ed44a3f0cb5"), "+381", "RS", "Serbia", 193 },
                    { new Guid("6843eab5-9ad3-4407-ad26-dc05530b24b6"), "+248", "SC", "Seychelles", 194 },
                    { new Guid("124822d8-a282-4251-98a2-8d8d5d90974f"), "+232", "SL", "Sierra Leone", 195 },
                    { new Guid("dc45dcfa-69ef-4e8c-99d0-9830a1c1547c"), "+1721", "SX", "Sint Maarten (Dutch part)", 196 },
                    { new Guid("1c00eee4-f8e7-4305-aa91-697cb92e499a"), "+421", "SK", "Slovakia", 197 },
                    { new Guid("b9989a34-bc0a-4d49-85d0-642ee4ee3685"), "+386", "SI", "Slovenia", 198 },
                    { new Guid("af0202a7-6d0d-45ad-bda8-3fbb83e1fc5f"), "+677", "SB", "Solomon Islands", 199 },
                    { new Guid("2d6157d3-44c7-41c3-9ade-27ad9bcedeae"), "+252", "SO", "Somalia", 200 },
                    { new Guid("49d77c61-000c-4f9e-96b8-d91d6ef3f492"), "+27", "ZA", "South Africa", 201 },
                    { new Guid("6ca02f50-247f-4f2b-b7ac-7df71ea8b299"), "+500", "GS", "South Georgia and the South Sandwich Islands", 202 },
                    { new Guid("7576f6d0-6efe-4b56-b5c4-c0a2cc754373"), "+211", "SS", "South Sudan", 203 },
                    { new Guid("6aafc3b9-4d76-4785-b62b-c31920a371f9"), "+34", "ES", "Spain", 204 },
                    { new Guid("622011ac-f9e1-4eeb-8486-fb568c4adb30"), "+94", "LK", "Sri Lanka", 205 },
                    { new Guid("fe115067-b0b0-4a64-be63-bcfd3df92783"), "+211", "SS", "Sudan", 206 },
                    { new Guid("7591d14a-359e-405d-ba38-1b7263a330db"), "+597", "SR", "Suriname", 207 },
                    { new Guid("a4d08888-0fa6-4a28-83be-aa31a9750db6"), "+4779", "SJ", "Svalbard and Jan Mayen", 208 },
                    { new Guid("258e17c3-23c7-4cb4-b971-de29d639f755"), "+268", "SZ", "Swaziland", 209 },
                    { new Guid("61fb06aa-e796-409c-8d8e-6a57562d7a5f"), "+46", "SE", "Sweden", 210 },
                    { new Guid("133cacf8-298e-4cbb-9047-5ea54451ce35"), "+41", "CH", "Switzerland", 211 },
                    { new Guid("0fce8bd4-f3d1-4b0f-af0e-76bac7f924d8"), "+266", "LS", "Lesotho", 124 },
                    { new Guid("30ff60d3-9b6d-47c1-8646-2b700bc215c0"), "+961", "LB", "Lebanon", 123 },
                    { new Guid("7a4c74c9-8322-4cb5-a842-cbab9ef65638"), "+371", "LV", "Latvia", 122 },
                    { new Guid("449cf3ad-2b52-4365-8b7a-012dc58ac07e"), "+53", "CU", "Cuba", 60 },
                    { new Guid("aa700eb5-9eb8-4d18-a76c-85137fc05275"), "+229", "BJ", "Benin", 34 },
                    { new Guid("f6e42fcd-0792-4f46-8d13-ece8dd27961d"), "+1441", "BM", "Bermuda", 35 },
                    { new Guid("15fd92c8-24ef-45ce-a77c-2b1051199332"), "+975", "BT", "Bhutan", 36 },
                    { new Guid("d823fb2a-d64b-4a91-aba6-41dfc468cebe"), "+5997", "BQ", "Bonaire, Sint Eustatius and Saba", 37 },
                    { new Guid("d8a175ad-0ab7-4172-9ff4-6807541e9a6e"), "+387", "BA", "Bosnia and Herzegovina", 38 },
                    { new Guid("9f95fa83-0d20-4d09-8179-6f87d0477881"), "+267", "BW", "Botswana", 39 },
                    { new Guid("2b8e4fac-0cf2-4b58-a666-3d5cab676974"), "+55", "BR", "Brazil", 40 },
                    { new Guid("3cbbee83-1692-4f8e-bfee-4b4e3cae5cae"), "+246", "IO", "British Indian Ocean Territory", 41 },
                    { new Guid("03d5089c-ddc9-417a-90db-a63e57910cfc"), "+359", "BG", "Bulgaria", 42 },
                    { new Guid("75bf37fd-73ec-40b4-bf92-1db1e5148493"), "+226", "BF", "Burkina Faso", 43 },
                    { new Guid("b0446b7b-1378-411e-890e-4375546a87e4"), "+237", "CM", "Cameroon", 44 },
                    { new Guid("11680b04-263d-4618-b2c0-ccbe38dc0a13"), "+1", "CA", "Canada", 45 },
                    { new Guid("ed992cb1-4438-47b1-9848-6878cb2562c0"), "+501", "BZ", "Belize", 33 },
                    { new Guid("74cea6e7-ccf2-44ad-9657-2f73cbb6a5f1"), "+1345", "KY", "Cayman Islands", 46 },
                    { new Guid("f3743a0e-7cb6-45b6-8ec1-e1fc0709ddf5"), "+235", "TD", "Chad", 48 },
                    { new Guid("26adaf42-dfec-41da-a61b-ffc141550f6f"), "+56", "CL", "Chile", 49 },
                    { new Guid("0d402363-b599-4fc6-95fc-a62bcc607392"), "+86", "CN", "China", 50 },
                    { new Guid("e7127643-928e-4d7d-ba22-56922376699c"), "+61", "CX", "Christmas Island", 51 },
                    { new Guid("895f09d1-c2a0-4028-a524-f797fe2dc6e0"), "+61", "CC", "Cocos (Keeling) Island", 52 },
                    { new Guid("840af5ec-854c-4ffd-b30a-48a8d50e8a39"), "+57", "CO", "Colombia", 53 },
                    { new Guid("fd335e4b-f0f9-4a7d-b405-20405291f77b"), "+269", "KM", "Comoros", 54 },
                    { new Guid("16c9a1bf-72d5-4234-a5e3-af5a9414b6e2"), "+242", "CG", "Congo, the Democratic Republic of the", 55 },
                    { new Guid("ac89fb2c-13bd-49e4-8211-abe9fe580e89"), "+682", "CK", "Cook Islands", 56 },
                    { new Guid("6ee34d44-9a71-4a81-9591-8a497e4caf83"), "+506", "CR", "Costa Rica", 57 },
                    { new Guid("6f7f51d8-e383-4783-85f4-cab416594b87"), "+225", "CI", "Côte d'Ivoire", 58 },
                    { new Guid("27354b5d-bb9c-4fa2-925a-8276cfb69a25"), "+385", "HR", "Croatia", 59 },
                    { new Guid("490d04b2-ff79-4934-96d0-26d0c203c719"), "+236", "CF", "Central African Republic", 47 },
                    { new Guid("358069d6-7c11-4384-9e23-65cda77efbd7"), "+996", "KG", "Kyrgyzstan", 121 },
                    { new Guid("873bb620-16b8-433f-88ed-b300ad7ca703"), "+32", "BE", "Belgium", 32 },
                    { new Guid("17786295-a189-4e90-a2f1-97d37dcd1805"), "+1246", "BB", "Barbados", 30 },
                    { new Guid("880cc2d6-8d5a-4c09-a540-d8a1c2c479ed"), "+62", "ID", "Indonesia", 4 },
                    { new Guid("681764c5-79b2-44fa-8c5d-600f0a4fbc60"), "+63", "PH", "Philippines", 5 },
                    { new Guid("6605f354-9bfb-41f6-8781-df8d072e374c"), "+84", "VN", "Vietnam", 6 },
                    { new Guid("bd929d16-30e4-4800-8683-cd372a64c2fc"), "+856", "LA", "Laos", 7 },
                    { new Guid("bb9f07cd-1691-4234-b7a7-95020b5e61c2"), "+855", "KH", "Cambodia", 8 },
                    { new Guid("7bc962ef-d6a8-4464-94c7-adc9d81e7663"), "+95", "MM", "Myanmar", 9 },
                    { new Guid("963ca351-7ae3-48d8-81f1-f42db181d9c0"), "+673", "BN", "Brunei Darussalam", 10 },
                    { new Guid("182fdca9-6da0-4bfb-be7b-4581926e1a1d"), "+93", "AF", "Afghanistan", 11 },
                    { new Guid("4d04bc1a-4103-470b-944e-95a16dec12dd"), "+358", "AX", "Åland Islands", 12 },
                    { new Guid("54df91ec-2682-43ba-8c95-e836a0bcffbd"), "+355", "AL", "Albania", 13 },
                    { new Guid("492ff976-dda1-43bd-9f21-900ecb0a8edc"), "+213", "DZ", "Algeria", 14 },
                    { new Guid("a3579034-8c90-43ee-8673-9cdb9abbb78d"), "+1684", "AS", "American Samoa", 15 },
                    { new Guid("22f59f1f-6ec3-4deb-8d5d-76f593011ac7"), "+375", "BY", "Belarus", 31 },
                    { new Guid("ff8bc647-db6e-433f-b16f-a5a4a7ac802a"), "+376", "AD", "Andorra", 16 },
                    { new Guid("c7daf42e-3aaf-4034-ada7-be70994b5ae5"), "+1264", "AI", "Anguilla", 18 },
                    { new Guid("11b9ac5a-ff89-4438-84f5-284b1bb84895"), "+672", "AQ", "Antarctica", 19 },
                    { new Guid("866260f3-be96-4992-ad4c-b7d2ca658233"), "+1268", "AG", "Antigua and Barbuda", 20 },
                    { new Guid("60edb818-b63b-455d-a99f-46dfe4912395"), "+54", "AR", "Argentina", 21 },
                    { new Guid("dc52253c-0aae-449e-862e-7831e72e472c"), "+374", "AM", "Armenia", 22 },
                    { new Guid("5233fce5-82ce-4492-a878-ad2ff5e2e16b"), "+297", "AW", "Aruba", 23 },
                    { new Guid("13288a94-b232-4e1e-8e87-e33d77678006"), "+61", "AU", "Australia", 24 },
                    { new Guid("41698544-7bb9-4b08-b99f-7afbdf50162f"), "+43", "AT", "Austria", 25 },
                    { new Guid("4e2e2dbf-9fe0-47fb-8296-7975b3a0ba77"), "+994", "AZ", "Azerbaijan", 26 },
                    { new Guid("a243ca69-0b48-419b-be6e-87fd1a3f1ae6"), "+973", "BH", "Bahrain", 27 },
                    { new Guid("81ff774f-bdd1-4d10-84ea-3581e555dc95"), "+1242", "BS", "Bahamas", 28 },
                    { new Guid("5d1b0c1f-0e07-477c-9f6e-78883d03828f"), "+880", "BD", "Bangladesh", 29 },
                    { new Guid("f9495419-1a1a-43c5-91f8-815249dd4ccc"), "+244", "AO", "Angola", 17 },
                    { new Guid("32e0895b-6190-4bf8-802d-9fac1ecc4793"), "+599", "CW", "Curaçao", 61 },
                    { new Guid("bbaf6d82-3d1d-4aaf-bb41-f89a8d75ed5c"), "+357", "CY", "Cyprus", 62 },
                    { new Guid("abd7a357-88bd-4c95-a867-ae033ebd455c"), "+420", "CZ", "Czech Republic", 63 },
                    { new Guid("645c8274-bfae-4d80-9ddb-ec3688966820"), "+240", "GQ", "Guinea", 95 },
                    { new Guid("d8d06115-fbaa-463d-b37c-78b3c29643f2"), "+245", "GW", "Guinea-Bissau", 96 },
                    { new Guid("753f8790-ba90-41ce-a344-9aaaf11379e1"), "+592", "GY", "Guyana", 97 },
                    { new Guid("93d94f5f-ca6f-4ef2-8d98-a38f572ca6f8"), "+509", "HT", "Haiti", 98 },
                    { new Guid("ea2b5169-5ffc-44d3-b2a3-dc8e484b53ee"), "+379", "VA", "Holy See (Vatican City State)", 99 },
                    { new Guid("939c18a1-64ed-4f36-857b-da2224f10d91"), "+504", "HN", "Honduras", 100 },
                    { new Guid("6664b53f-fa7e-4368-9d8b-19128029013f"), "+852", "HK", "Hong Kong", 101 },
                    { new Guid("c1b4d912-a0a2-4d6b-996c-5a46088e95cf"), "+36", "HU", "Hungary", 102 },
                    { new Guid("3d1a8091-ac74-44e2-9a16-55311443f5c0"), "+354", "IS", "Iceland", 103 },
                    { new Guid("cae8330e-217d-40a1-842c-a488d0feff90"), "+246", "IO", "India", 104 },
                    { new Guid("f7adf147-1ae2-4b95-9446-41c7a906638a"), "+98", "IR", "Iran, Islamic Republic of", 105 },
                    { new Guid("2189cbf9-17d0-4724-8ff7-59631c1cffd4"), "+964", "IQ", "Iraq", 106 },
                    { new Guid("a54158db-9f9c-4256-8675-564e565238dd"), "+44", "GG", "Guernsey", 94 },
                    { new Guid("e247b6aa-8936-46bf-8185-ff04840b30bd"), "+353", "IE", "Ireland", 107 },
                    { new Guid("8c6d39a5-119f-4c93-8196-17ee320994db"), "+972", "IL", "Israel", 109 },
                    { new Guid("9f01fb8f-81d6-4827-9556-055e85ff7699"), "+39", "IT", "Italy", 110 },
                    { new Guid("8d0b1391-e6c0-4e09-9d51-f80948fe68ad"), "+1876", "JM", "Jamaica", 111 },
                    { new Guid("b4b1ec2f-e627-4dcf-b986-bd70f5116504"), "+81", "JP", "Japan", 112 },
                    { new Guid("bfb007a3-c217-49a1-8640-73f71c5ed97f"), "+44", "JE", "Jersey", 113 },
                    { new Guid("960e1739-1f27-47be-963b-20e4d93e1a4a"), "+962", "JO", "Jordan", 114 },
                    { new Guid("98de434a-d46f-42b1-9ba9-02fbf1e786a0"), "+76", "KZ", "Kazakhstan", 115 },
                    { new Guid("356de4ad-5c94-4798-911a-b9dee48ba892"), "+254", "KE", "Kenya", 116 },
                    { new Guid("1869cfea-588b-4456-aa95-4d7f11c372b5"), "+686", "KI", "Kiribati", 117 },
                    { new Guid("0c833c66-d34a-4997-85f7-77a2e119f505"), "+850", "KP", "North Korea, Democratic People's Republic of", 118 },
                    { new Guid("f900fa6c-7483-4247-a08a-f912134e9a88"), "+82", "KR", "Korea, Republic of", 119 },
                    { new Guid("2a0f238e-f37f-4274-ade0-f74d3504da8b"), "+965", "KW", "Kuwait", 120 },
                    { new Guid("b2ab5e94-32da-48f3-869f-c4ed772e767a"), "+44", "IM", "Isle of Man", 108 },
                    { new Guid("0bc9a157-1dd0-4c49-bf1b-1ac9ea37892e"), "+502", "GT", "Guatemala", 93 },
                    { new Guid("219333d8-9c01-4dd4-abeb-1c6f26161243"), "+1671", "GU", "Guam", 92 },
                    { new Guid("48a2ea82-dd2c-40cd-a276-c7af105c27f0"), "+590", "GP", "Guadeloupe", 91 },
                    { new Guid("6892b71e-265a-4b30-b76a-c4e4716ac9fa"), "+45", "DK", "Denmark", 64 },
                    { new Guid("80ba33bd-f4ef-471a-8ee9-3f91e5d95186"), "+253", "DJ", "Djibouti", 65 },
                    { new Guid("8c2dc150-8295-414a-afdf-f64c9ebe80f7"), "+1767", "DM", "Dominica", 66 },
                    { new Guid("2627da0d-030f-4dcc-be9d-04030ebf9198"), "+1809", "DO", "Dominican Republic", 67 },
                    { new Guid("f7cf6470-2cfc-4e56-a719-c3feb086583c"), "+593", "EC", "Ecuador", 68 },
                    { new Guid("c29b18d1-1cd1-4329-a0dc-a00ed6a18a61"), "+20", "EG", "Egypt", 69 },
                    { new Guid("825b15ef-4cf2-492a-9470-e1be228a1120"), "+503", "SV", "El Salvador", 70 },
                    { new Guid("861709cd-1e71-4c42-b899-074a3036cf54"), "+240", "GQ", "Equatorial Guinea", 71 },
                    { new Guid("5a381645-56e1-4cda-b143-829b83979833"), "+291", "ER", "Eritrea", 72 },
                    { new Guid("3d6a6306-fdd1-43ae-ac31-dd84a03f0caa"), "+372", "EE", "Estonia", 73 },
                    { new Guid("fd0dce00-bdf1-467e-9f83-b86fcba1eed0"), "+251", "ET", "Ethiopia", 74 },
                    { new Guid("76a801c8-bb1b-4eaf-987d-f07ec1e74efa"), "+500", "FK", "Falkland Islands (Malvinas)", 75 },
                    { new Guid("2dd1fa4e-bfb4-4e5a-8bf5-12496042aee7"), "+298", "FO", "Faroe Islands", 76 },
                    { new Guid("6473321e-e100-42e8-9ed2-156fd33da27f"), "+679", "FJ", "Fiji", 77 },
                    { new Guid("96ba6821-d3a6-49d6-9e5f-58d9ab46d084"), "+358", "FI", "Finland", 78 },
                    { new Guid("81c2671a-aeb4-4721-b11f-ca84e5ec137d"), "+33", "FR", "France", 79 },
                    { new Guid("24dd849e-967c-437c-abab-9be36a38dfe6"), "+594", "GF", "French Guiana", 80 },
                    { new Guid("a1e421fe-7fa3-4efa-bfcc-95e347239f5b"), "+689", "PF", "French Polynesia", 81 },
                    { new Guid("a0eaacd7-a223-4438-9c3e-47d1982c568b"), "+241", "GA", "Gabon", 82 },
                    { new Guid("2cfe1eda-1b31-4ccb-9e03-827d36e188e0"), "+220", "GM", "Gambia", 83 },
                    { new Guid("7d17e262-3fce-4dd8-a00f-7b8b02da4073"), "+995", "GE", "Georgia", 84 },
                    { new Guid("78b7fb49-70df-4d84-8470-4539596ba8e1"), "+49", "DE", "Germany", 85 },
                    { new Guid("b85d205d-a8c4-48ef-b98b-7891a6a1b645"), "+233", "GH", "Ghana", 86 },
                    { new Guid("bca18f4c-7290-4f4c-8611-b8a71108ea38"), "+350", "GI", "Gibraltar", 87 },
                    { new Guid("f4991e89-086b-414c-beae-e885ef0f3c66"), "+30", "GR", "Greece", 88 },
                    { new Guid("4717e9d5-3b56-460f-b29d-9cbaf6157727"), "+299", "GL", "Greenland", 89 },
                    { new Guid("e70f895b-121f-4453-9be7-90db874c2271"), "+1473", "GD", "Grenada", 90 },
                    { new Guid("9aaa7914-d44e-4356-af26-6d07c498d0d3"), "+66", "TH", "Thailand", 3 },
                    { new Guid("b3f8d93a-630f-4b3f-8f43-5ba0a64b79d1"), "+65", "SG", "Singapore", 2 }
                });

            migrationBuilder.InsertData(
                table: "Gender",
                columns: new[] { "ID", "Code", "Name" },
                values: new object[,]
                {
                    { new Guid("a74e7e41-dea4-4c67-add6-785735717cdc"), "M", "Male" },
                    { new Guid("1af6209b-5cf1-408e-b89b-4bdf8c302c09"), "F", "Female" },
                    { new Guid("8b6a4475-cda7-4b33-a0f2-8839f755a799"), "U", "Unknown" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_CountryID",
                table: "Address",
                column: "CountryID");

            migrationBuilder.CreateIndex(
                name: "IX_Address_PersonID",
                table: "Address",
                column: "PersonID");

            migrationBuilder.CreateIndex(
                name: "IX_Gender_Code",
                table: "Gender",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Person_GenderID",
                table: "Person",
                column: "GenderID");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneNumber_CountryID",
                table: "PhoneNumber",
                column: "CountryID");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneNumber_PersonID",
                table: "PhoneNumber",
                column: "PersonID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "PhoneNumber");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "Gender");
        }
    }
}
