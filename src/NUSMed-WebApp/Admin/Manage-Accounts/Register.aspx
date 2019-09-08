<%@ Page Title="Account Registration" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="NUSMed_WebApp.Register" %>

<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="container">
        <div class="py-5 mx-auto text-center">
            <h1 class="display-4"><i class="fas fa-fw fa-user-plus"></i>Account Registration</h1>
            <p class="lead">Restricted. This page is meant for administrators only.</p>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanelRegistration" class="container" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="py-1 mx-auto">
                <h1 class="display-5">Personal Information</h1>
            </div>
            <div class="row">
                <div class="col-12 col-md-6">
                    <div class="form-group">
                        <label for="inputNRIC">NRIC</label>
                        <input id="inputNRIC" type="text" class="form-control" placeholder="NRIC" required="required" runat="server">
                        <div class="invalid-feedback" runat="server">
                            NRIC is invalid or has already been registered.
                        </div>
                    </div>
                </div>
                <div class="col-12 col-md-6">
                    <div class="form-group">
                        <label for="inputDoB">Date of Birth</label>
                        <input id="inputDoB" name="dateOfBirth" type="date" class="form-control" placeholder="Date of Birth" required="required" runat="server">
                        <div class="invalid-feedback" runat="server">
                            Date of Birth is invalid.
                        </div>
                    </div>
                </div>
                <div class="col-12 col-md-6">
                    <div class="form-group">
                        <label for="inputFirstName">First Name</label>
                        <input id="inputFirstName" type="text" class="form-control" placeholder="First Name" required="required" runat="server">
                        <div class="invalid-feedback" runat="server">
                            First Name is invalid.
                        </div>
                    </div>
                </div>
                <div class="col-12 col-md-6">
                    <div class="form-group">
                        <label for="inputLastName">Last Name</label>
                        <input id="inputLastName" type="text" class="form-control" placeholder="Last Name" required="required" runat="server">
                        <div class="invalid-feedback" runat="server">
                            Last Name is invalid.
                        </div>
                    </div>
                </div>
                <div class="col-12 col-md-6">
                    <div class="form-group">
                        <label for="inputCountryofBirth">Country of Birth</label>
                        <select class="form-control" id="inputCountryofBirth" runat="server">
                            <option value="">-- select one --</option>
                            <option value="Afghanistan">Afghanistan</option>
                            <option value="Albania">Albania</option>
                            <option value="Algeria">Algeria</option>
                            <option value="Andorra">Andorra</option>
                            <option value="Antigua and Barbuda">Antigua and Barbuda</option>
                            <option value="Argentina">Argentina</option>
                            <option value="Armenia">Armenia</option>
                            <option value="Australia">Australia</option>
                            <option value="Austria">Austria</option>
                            <option value="Azerbaijan">Azerbaijan</option>
                            <option value="Bahamas">Bahamas</option>
                            <option value="Bahrain">Bahrain</option>
                            <option value="Bangladesh">Bangladesh</option>
                            <option value="Barbados">Barbados</option>
                            <option value="Belarus">Belarus</option>
                            <option value="Belgium">Belgium</option>
                            <option value="Belize">Belize</option>
                            <option value="Benin">Benin</option>
                            <option value="Bhutan">Bhutan</option>
                            <option value="Bolivia">Bolivia</option>
                            <option value="Bosnia and Herzegovina">Bosnia and Herzegovina</option>
                            <option value="Botswana">Botswana</option>
                            <option value="Brazil">Brazil</option>
                            <option value="Brunei">Brunei</option>
                            <option value="Bulgaria">Bulgaria</option>
                            <option value="Burkina Faso">Burkina Faso</option>
                            <option value="Burundi">Burundi</option>
                            <option value="Cambodia">Cambodia</option>
                            <option value="Cameroon">Cameroon</option>
                            <option value="Canada">Canada</option>
                            <option value="Cape Verde">Cape Verde</option>
                            <option value="Central African Republic">Central African Republic</option>
                            <option value="Chad">Chad</option>
                            <option value="Chile">Chile</option>
                            <option value="China">China</option>
                            <option value="Colombia">Colombia</option>
                            <option value="Comoros">Comoros</option>
                            <option value="Congo">Congo</option>
                            <option value="Costa Rica">Costa Rica</option>
                            <option value="Cote d'Ivoire">Cote d'Ivoire</option>
                            <option value="Croatia">Croatia</option>
                            <option value="Cuba">Cuba</option>
                            <option value="Cyprus">Cyprus</option>
                            <option value="Czech Republic">Czech Republic</option>
                            <option value="Denmark">Denmark</option>
                            <option value="Djibouti">Djibouti</option>
                            <option value="Dominica">Dominica</option>
                            <option value="Dominican Republic">Dominican Republic</option>
                            <option value="East Timor">East Timor</option>
                            <option value="Ecuador">Ecuador</option>
                            <option value="Egypt">Egypt</option>
                            <option value="El Salvador">El Salvador</option>
                            <option value="Equatorial Guinea">Equatorial Guinea</option>
                            <option value="Eritrea">Eritrea</option>
                            <option value="Estonia">Estonia</option>
                            <option value="Ethiopia">Ethiopia</option>
                            <option value="Fiji">Fiji</option>
                            <option value="Finland">Finland</option>
                            <option value="France">France</option>
                            <option value="Gabon">Gabon</option>
                            <option value="Gambia">Gambia</option>
                            <option value="Georgia">Georgia</option>
                            <option value="Germany">Germany</option>
                            <option value="Ghana">Ghana</option>
                            <option value="Greece">Greece</option>
                            <option value="Grenada">Grenada</option>
                            <option value="Guatemala">Guatemala</option>
                            <option value="Guinea">Guinea</option>
                            <option value="Guinea-Bissau">Guinea-Bissau</option>
                            <option value="Guyana">Guyana</option>
                            <option value="Haiti">Haiti</option>
                            <option value="Honduras">Honduras</option>
                            <option value="Hong Kong">Hong Kong</option>
                            <option value="Hungary">Hungary</option>
                            <option value="Iceland">Iceland</option>
                            <option value="India">India</option>
                            <option value="Indonesia">Indonesia</option>
                            <option value="Iran">Iran</option>
                            <option value="Iraq">Iraq</option>
                            <option value="Ireland">Ireland</option>
                            <option value="Israel">Israel</option>
                            <option value="Italy">Italy</option>
                            <option value="Jamaica">Jamaica</option>
                            <option value="Japan">Japan</option>
                            <option value="Jordan">Jordan</option>
                            <option value="Kazakhstan">Kazakhstan</option>
                            <option value="Kenya">Kenya</option>
                            <option value="Kiribati">Kiribati</option>
                            <option value="North Korea">North Korea</option>
                            <option value="South Korea">South Korea</option>
                            <option value="Kuwait">Kuwait</option>
                            <option value="Kyrgyzstan">Kyrgyzstan</option>
                            <option value="Laos">Laos</option>
                            <option value="Latvia">Latvia</option>
                            <option value="Lebanon">Lebanon</option>
                            <option value="Lesotho">Lesotho</option>
                            <option value="Liberia">Liberia</option>
                            <option value="Libya">Libya</option>
                            <option value="Liechtenstein">Liechtenstein</option>
                            <option value="Lithuania">Lithuania</option>
                            <option value="Luxembourg">Luxembourg</option>
                            <option value="Macedonia">Macedonia</option>
                            <option value="Madagascar">Madagascar</option>
                            <option value="Malawi">Malawi</option>
                            <option value="Malaysia">Malaysia</option>
                            <option value="Maldives">Maldives</option>
                            <option value="Mali">Mali</option>
                            <option value="Malta">Malta</option>
                            <option value="Marshall Islands">Marshall Islands</option>
                            <option value="Mauritania">Mauritania</option>
                            <option value="Mauritius">Mauritius</option>
                            <option value="Mexico">Mexico</option>
                            <option value="Micronesia">Micronesia</option>
                            <option value="Moldova">Moldova</option>
                            <option value="Monaco">Monaco</option>
                            <option value="Mongolia">Mongolia</option>
                            <option value="Montenegro">Montenegro</option>
                            <option value="Morocco">Morocco</option>
                            <option value="Mozambique">Mozambique</option>
                            <option value="Myanmar">Myanmar</option>
                            <option value="Namibia">Namibia</option>
                            <option value="Nauru">Nauru</option>
                            <option value="Nepal">Nepal</option>
                            <option value="Netherlands">Netherlands</option>
                            <option value="New Zealand">New Zealand</option>
                            <option value="Nicaragua">Nicaragua</option>
                            <option value="Niger">Niger</option>
                            <option value="Nigeria">Nigeria</option>
                            <option value="Norway">Norway</option>
                            <option value="Oman">Oman</option>
                            <option value="Pakistan">Pakistan</option>
                            <option value="Palau">Palau</option>
                            <option value="Panama">Panama</option>
                            <option value="Papua New Guinea">Papua New Guinea</option>
                            <option value="Paraguay">Paraguay</option>
                            <option value="Peru">Peru</option>
                            <option value="Philippines">Philippines</option>
                            <option value="Poland">Poland</option>
                            <option value="Portugal">Portugal</option>
                            <option value="Puerto Rico">Puerto Rico</option>
                            <option value="Qatar">Qatar</option>
                            <option value="Romania">Romania</option>
                            <option value="Russia">Russia</option>
                            <option value="Rwanda">Rwanda</option>
                            <option value="Saint Kitts and Nevis">Saint Kitts and Nevis</option>
                            <option value="Saint Lucia">Saint Lucia</option>
                            <option value="Saint Vincent and the Grenadines">Saint Vincent and the Grenadines</option>
                            <option value="Samoa">Samoa</option>
                            <option value="San Marino">San Marino</option>
                            <option value="Sao Tome and Principe">Sao Tome and Principe</option>
                            <option value="Saudi Arabia">Saudi Arabia</option>
                            <option value="Senegal">Senegal</option>
                            <option value="Serbia and Montenegro">Serbia and Montenegro</option>
                            <option value="Seychelles">Seychelles</option>
                            <option value="Sierra Leone">Sierra Leone</option>
                            <option value="Singapore">Singapore</option>
                            <option value="Slovakia">Slovakia</option>
                            <option value="Slovenia">Slovenia</option>
                            <option value="Solomon Islands">Solomon Islands</option>
                            <option value="Somalia">Somalia</option>
                            <option value="South Africa">South Africa</option>
                            <option value="Spain">Spain</option>
                            <option value="Sri Lanka">Sri Lanka</option>
                            <option value="Sudan">Sudan</option>
                            <option value="Suriname">Suriname</option>
                            <option value="Swaziland">Swaziland</option>
                            <option value="Sweden">Sweden</option>
                            <option value="Switzerland">Switzerland</option>
                            <option value="Syria">Syria</option>
                            <option value="Taiwan">Taiwan</option>
                            <option value="Tajikistan">Tajikistan</option>
                            <option value="Tanzania">Tanzania</option>
                            <option value="Thailand">Thailand</option>
                            <option value="Togo">Togo</option>
                            <option value="Tonga">Tonga</option>
                            <option value="Trinidad and Tobago">Trinidad and Tobago</option>
                            <option value="Tunisia">Tunisia</option>
                            <option value="Turkey">Turkey</option>
                            <option value="Turkmenistan">Turkmenistan</option>
                            <option value="Tuvalu">Tuvalu</option>
                            <option value="Uganda">Uganda</option>
                            <option value="Ukraine">Ukraine</option>
                            <option value="United Arab Emirates">United Arab Emirates</option>
                            <option value="United Kingdom">United Kingdom</option>
                            <option value="United States">United States</option>
                            <option value="Uruguay">Uruguay</option>
                            <option value="Uzbekistan">Uzbekistan</option>
                            <option value="Vanuatu">Vanuatu</option>
                            <option value="Vatican City">Vatican City</option>
                            <option value="Venezuela">Venezuela</option>
                            <option value="Vietnam">Vietnam</option>
                            <option value="Yemen">Yemen</option>
                            <option value="Zambia">Zambia</option>
                            <option value="Zimbabwe">Zimbabwe</option>
                        </select>
                        <div class="invalid-feedback" runat="server">
                            Country of Birth is invalid.
                        </div>
                    </div>
                </div>
                <div class="col-12 col-md-6">
                    <div class="form-group">
                        <label for="inputNationality">Nationality</label>
                        <select class="form-control" id="inputNationality" runat="server">
                            <option value="">-- select one --</option>
                            <option value="afghan">Afghan</option>
                            <option value="albanian">Albanian</option>
                            <option value="algerian">Algerian</option>
                            <option value="american">American</option>
                            <option value="andorran">Andorran</option>
                            <option value="angolan">Angolan</option>
                            <option value="antiguans">Antiguans</option>
                            <option value="argentinean">Argentinean</option>
                            <option value="armenian">Armenian</option>
                            <option value="australian">Australian</option>
                            <option value="austrian">Austrian</option>
                            <option value="azerbaijani">Azerbaijani</option>
                            <option value="bahamian">Bahamian</option>
                            <option value="bahraini">Bahraini</option>
                            <option value="bangladeshi">Bangladeshi</option>
                            <option value="barbadian">Barbadian</option>
                            <option value="barbudans">Barbudans</option>
                            <option value="batswana">Batswana</option>
                            <option value="belarusian">Belarusian</option>
                            <option value="belgian">Belgian</option>
                            <option value="belizean">Belizean</option>
                            <option value="beninese">Beninese</option>
                            <option value="bhutanese">Bhutanese</option>
                            <option value="bolivian">Bolivian</option>
                            <option value="bosnian">Bosnian</option>
                            <option value="brazilian">Brazilian</option>
                            <option value="british">British</option>
                            <option value="bruneian">Bruneian</option>
                            <option value="bulgarian">Bulgarian</option>
                            <option value="burkinabe">Burkinabe</option>
                            <option value="burmese">Burmese</option>
                            <option value="burundian">Burundian</option>
                            <option value="cambodian">Cambodian</option>
                            <option value="cameroonian">Cameroonian</option>
                            <option value="canadian">Canadian</option>
                            <option value="cape verdean">Cape Verdean</option>
                            <option value="central african">Central African</option>
                            <option value="chadian">Chadian</option>
                            <option value="chilean">Chilean</option>
                            <option value="chinese">Chinese</option>
                            <option value="colombian">Colombian</option>
                            <option value="comoran">Comoran</option>
                            <option value="congolese">Congolese</option>
                            <option value="costa rican">Costa Rican</option>
                            <option value="croatian">Croatian</option>
                            <option value="cuban">Cuban</option>
                            <option value="cypriot">Cypriot</option>
                            <option value="czech">Czech</option>
                            <option value="danish">Danish</option>
                            <option value="djibouti">Djibouti</option>
                            <option value="dominican">Dominican</option>
                            <option value="dutch">Dutch</option>
                            <option value="east timorese">East Timorese</option>
                            <option value="ecuadorean">Ecuadorean</option>
                            <option value="egyptian">Egyptian</option>
                            <option value="emirian">Emirian</option>
                            <option value="equatorial guinean">Equatorial Guinean</option>
                            <option value="eritrean">Eritrean</option>
                            <option value="estonian">Estonian</option>
                            <option value="ethiopian">Ethiopian</option>
                            <option value="fijian">Fijian</option>
                            <option value="filipino">Filipino</option>
                            <option value="finnish">Finnish</option>
                            <option value="french">French</option>
                            <option value="gabonese">Gabonese</option>
                            <option value="gambian">Gambian</option>
                            <option value="georgian">Georgian</option>
                            <option value="german">German</option>
                            <option value="ghanaian">Ghanaian</option>
                            <option value="greek">Greek</option>
                            <option value="grenadian">Grenadian</option>
                            <option value="guatemalan">Guatemalan</option>
                            <option value="guinea-bissauan">Guinea-Bissauan</option>
                            <option value="guinean">Guinean</option>
                            <option value="guyanese">Guyanese</option>
                            <option value="haitian">Haitian</option>
                            <option value="herzegovinian">Herzegovinian</option>
                            <option value="honduran">Honduran</option>
                            <option value="hungarian">Hungarian</option>
                            <option value="icelander">Icelander</option>
                            <option value="indian">Indian</option>
                            <option value="indonesian">Indonesian</option>
                            <option value="iranian">Iranian</option>
                            <option value="iraqi">Iraqi</option>
                            <option value="irish">Irish</option>
                            <option value="israeli">Israeli</option>
                            <option value="italian">Italian</option>
                            <option value="ivorian">Ivorian</option>
                            <option value="jamaican">Jamaican</option>
                            <option value="japanese">Japanese</option>
                            <option value="jordanian">Jordanian</option>
                            <option value="kazakhstani">Kazakhstani</option>
                            <option value="kenyan">Kenyan</option>
                            <option value="kittian and nevisian">Kittian and Nevisian</option>
                            <option value="kuwaiti">Kuwaiti</option>
                            <option value="kyrgyz">Kyrgyz</option>
                            <option value="laotian">Laotian</option>
                            <option value="latvian">Latvian</option>
                            <option value="lebanese">Lebanese</option>
                            <option value="liberian">Liberian</option>
                            <option value="libyan">Libyan</option>
                            <option value="liechtensteiner">Liechtensteiner</option>
                            <option value="lithuanian">Lithuanian</option>
                            <option value="luxembourger">Luxembourger</option>
                            <option value="macedonian">Macedonian</option>
                            <option value="malagasy">Malagasy</option>
                            <option value="malawian">Malawian</option>
                            <option value="malaysian">Malaysian</option>
                            <option value="maldivan">Maldivan</option>
                            <option value="malian">Malian</option>
                            <option value="maltese">Maltese</option>
                            <option value="marshallese">Marshallese</option>
                            <option value="mauritanian">Mauritanian</option>
                            <option value="mauritian">Mauritian</option>
                            <option value="mexican">Mexican</option>
                            <option value="micronesian">Micronesian</option>
                            <option value="moldovan">Moldovan</option>
                            <option value="monacan">Monacan</option>
                            <option value="mongolian">Mongolian</option>
                            <option value="moroccan">Moroccan</option>
                            <option value="mosotho">Mosotho</option>
                            <option value="motswana">Motswana</option>
                            <option value="mozambican">Mozambican</option>
                            <option value="namibian">Namibian</option>
                            <option value="nauruan">Nauruan</option>
                            <option value="nepalese">Nepalese</option>
                            <option value="new zealander">New Zealander</option>
                            <option value="ni-vanuatu">Ni-Vanuatu</option>
                            <option value="nicaraguan">Nicaraguan</option>
                            <option value="nigerien">Nigerien</option>
                            <option value="north korean">North Korean</option>
                            <option value="northern irish">Northern Irish</option>
                            <option value="norwegian">Norwegian</option>
                            <option value="omani">Omani</option>
                            <option value="pakistani">Pakistani</option>
                            <option value="palauan">Palauan</option>
                            <option value="panamanian">Panamanian</option>
                            <option value="papua new guinean">Papua New Guinean</option>
                            <option value="paraguayan">Paraguayan</option>
                            <option value="peruvian">Peruvian</option>
                            <option value="polish">Polish</option>
                            <option value="portuguese">Portuguese</option>
                            <option value="qatari">Qatari</option>
                            <option value="romanian">Romanian</option>
                            <option value="russian">Russian</option>
                            <option value="rwandan">Rwandan</option>
                            <option value="saint lucian">Saint Lucian</option>
                            <option value="salvadoran">Salvadoran</option>
                            <option value="samoan">Samoan</option>
                            <option value="san marinese">San Marinese</option>
                            <option value="sao tomean">Sao Tomean</option>
                            <option value="saudi">Saudi</option>
                            <option value="scottish">Scottish</option>
                            <option value="senegalese">Senegalese</option>
                            <option value="serbian">Serbian</option>
                            <option value="seychellois">Seychellois</option>
                            <option value="sierra leonean">Sierra Leonean</option>
                            <option value="singaporean">Singaporean</option>
                            <option value="slovakian">Slovakian</option>
                            <option value="slovenian">Slovenian</option>
                            <option value="solomon islander">Solomon Islander</option>
                            <option value="somali">Somali</option>
                            <option value="south african">South African</option>
                            <option value="south korean">South Korean</option>
                            <option value="spanish">Spanish</option>
                            <option value="sri lankan">Sri Lankan</option>
                            <option value="sudanese">Sudanese</option>
                            <option value="surinamer">Surinamer</option>
                            <option value="swazi">Swazi</option>
                            <option value="swedish">Swedish</option>
                            <option value="swiss">Swiss</option>
                            <option value="syrian">Syrian</option>
                            <option value="taiwanese">Taiwanese</option>
                            <option value="tajik">Tajik</option>
                            <option value="tanzanian">Tanzanian</option>
                            <option value="thai">Thai</option>
                            <option value="togolese">Togolese</option>
                            <option value="tongan">Tongan</option>
                            <option value="trinidadian or tobagonian">Trinidadian or Tobagonian</option>
                            <option value="tunisian">Tunisian</option>
                            <option value="turkish">Turkish</option>
                            <option value="tuvaluan">Tuvaluan</option>
                            <option value="ugandan">Ugandan</option>
                            <option value="ukrainian">Ukrainian</option>
                            <option value="uruguayan">Uruguayan</option>
                            <option value="uzbekistani">Uzbekistani</option>
                            <option value="venezuelan">Venezuelan</option>
                            <option value="vietnamese">Vietnamese</option>
                            <option value="welsh">Welsh</option>
                            <option value="yemenite">Yemenite</option>
                            <option value="zambian">Zambian</option>
                            <option value="zimbabwean">Zimbabwean</option>
                        </select>
                        <div class="invalid-feedback" runat="server">
                            Nationality is invalid.
                        </div>
                    </div>
                </div>

                <div class="col-12 col-xl-6">
                    <div class="form-group">
                        <label>Sex</label>
                        <br />
                        <div class="form-check form-check-inline">
                            <asp:RadioButton ID="RadioButtonSexMale" CssClass="form-check-input" runat="server" GroupName="RadioButtonSex" Checked="true" />
                            <label class="form-check-label" for="<%= RadioButtonSexMale.ClientID %>">Male</label>
                        </div>
                        <div class="form-check form-check-inline">
                            <asp:RadioButton ID="RadioButtonSexFemale" CssClass="form-check-input" runat="server" GroupName="RadioButtonSex" />
                            <label class="form-check-label" for="<%= RadioButtonSexFemale.ClientID %>">Female</label>
                        </div>
                    </div>
                </div>
                <div class="col-12 col-xl-6">
                    <div class="form-group">
                        <label>Gender</label>
                        <br />
                        <div class="form-check form-check-inline">
                            <asp:RadioButton ID="RadioButtonGenderMale" CssClass="form-check-input" runat="server" GroupName="RadioButtonGender" Checked="true" />
                            <label class="form-check-label" for="<%= RadioButtonGenderMale.ClientID %>">Male</label>
                        </div>
                        <div class="form-check form-check-inline">
                            <asp:RadioButton ID="RadioButtonGenderFemale" CssClass="form-check-input" runat="server" GroupName="RadioButtonGender" />
                            <label class="form-check-label" for="<%= RadioButtonGenderFemale.ClientID %>">Female</label>
                        </div>
                        <div class="form-check form-check-inline">
                            <asp:RadioButton ID="RadioButtonGenderIntersex" CssClass="form-check-input" runat="server" GroupName="RadioButtonGender" />
                            <label class="form-check-label" for="<%= RadioButtonGenderIntersex.ClientID %>">Intersex</label>
                        </div>
                        <div class="form-check form-check-inline">
                            <asp:RadioButton ID="RadioButtonGenderOther" CssClass="form-check-input" runat="server" GroupName="RadioButtonGender" />
                            <label class="form-check-label" for="<%= RadioButtonGenderOther.ClientID %>">Other</label>
                        </div>
                        <div class="form-check form-check-inline">
                            <asp:RadioButton ID="RadioButtonGenderPreferNot" CssClass="form-check-input" runat="server" GroupName="RadioButtonGender" />
                            <label class="form-check-label" for="<%= RadioButtonGenderPreferNot.ClientID %>">Prefer not to say</label>
                        </div>
                    </div>
                </div>

                <div class="col-12">
                    <div class="form-group">
                        <label>Martial Status</label>
                        <br />
                        <div class="form-check form-check-inline">
                            <asp:RadioButton ID="RadioButtonMartialStatusSingle" CssClass="form-check-input" runat="server" GroupName="RadioButtonMartialStatus" Checked="true" />
                            <label class="form-check-label" for="<%= RadioButtonMartialStatusSingle.ClientID %>">Single</label>
                        </div>
                        <div class="form-check form-check-inline">
                            <asp:RadioButton ID="RadioButtonMartialStatusMarried" CssClass="form-check-input" runat="server" GroupName="RadioButtonMartialStatus" />
                            <label class="form-check-label" for="<%= RadioButtonMartialStatusMarried.ClientID %>">Married</label>
                        </div>
                        <div class="form-check form-check-inline">
                            <asp:RadioButton ID="RadioButtonMartialStatusDivorced" CssClass="form-check-input" runat="server" GroupName="RadioButtonMartialStatus" />
                            <label class="form-check-label" for="<%= RadioButtonMartialStatusDivorced.ClientID %>">Divorced</label>
                        </div>
                        <div class="form-check form-check-inline">
                            <asp:RadioButton ID="RadioButtonMartialStatusWidowed" CssClass="form-check-input" runat="server" GroupName="RadioButtonMartialStatus" />
                            <label class="form-check-label" for="<%= RadioButtonMartialStatusWidowed.ClientID %>">Widowed</label>
                        </div>
                    </div>
                </div>
            </div>

            <div class="py-3 mx-auto">
                <h1 class="display-5">Contact Information</h1>
            </div>
            <div class="row">
                <div class="col-12 col-lg-8">
                    <div class="form-group">
                        <label for="inputAddress">Address</label>
                        <input id="inputAddress" type="text" class="form-control" placeholder="Address" required="required" runat="server">
                        <div class="invalid-feedback" runat="server">
                            Address is invalid.
                        </div>
                    </div>
                </div>
                <div class="col-12 col-lg-4">
                    <div class="form-group">
                        <label for="inputPostalCode">Postal Code <span class="text-muted small">(6-digits)</span></label>
                        <input id="inputPostalCode" type="text" class="form-control" placeholder="Postal Code" required="required" runat="server">
                        <div class="invalid-feedback" runat="server">
                            Postal Code is invalid.
                        </div>
                    </div>
                </div>
                <div class="col-12 col-lg-8">
                    <div class="form-group">
                        <label for="inputEmail">Email Address</label>
                        <input id="inputEmail" type="email" class="form-control" placeholder="Email Address" required="required" runat="server">
                        <div class="invalid-feedback" runat="server">
                            Email Address is invalid.
                        </div>
                    </div>
                </div>
                <div class="col-12 col-lg-4">
                    <div class="form-group">
                        <label for="inputContactNumber">Contact Number <span class="text-muted small">(8-digits)</span></label>
                        <input id="inputContactNumber" type="tel" class="form-control" placeholder="Contact Number" required="required" runat="server">
                        <div class="invalid-feedback" runat="server">
                            Contact Number is invalid.
                        </div>
                    </div>
                </div>
            </div>

            <div class="py-3 mx-auto">
                <h1 class="display-5">Authentication <span class="text-muted small">(1FA and MFA)</span></h1>
            </div>
            <div class="row">
                <div class="col-12 col-lg-6">
                    <div class="form-group">
                        <label for="inputPassword">Password <small class="text-muted">(Has to contain symbols, digits and 12 characters in length)</small></label>
                        <input id="inputPassword" type="password" class="form-control" placeholder="Password" runat="server">
                    </div>
                </div>
                <div class="col-12 col-lg-6">
                    <div class="form-group">
                        <label for="inputPasswordConfirm">Confirm Password</label>
                        <input id="inputPasswordConfirm" type="password" class="form-control" placeholder="Confirm Password" runat="server">
                        <div class="invalid-feedback" runat="server">
                            Password does not meet requirements or does not match.
                        </div>
                    </div>
                </div>
                <div class="col-12">
                    <div class="form-group">
                        <label for="inputAssociatedTokenID">Associated Token Device ID <small class="text-muted">(Leave blank to set account to be Omitted MFA)</small></label>
                        <input id="inputAssociatedTokenID" type="text" class="form-control" placeholder="Associated Token Device ID" runat="server">
                        <div class="invalid-feedback" runat="server">
                            Associated Token ID is invalid.
                        </div>
                    </div>
                </div>
            </div>

            <div class="py-3 mx-auto">
                <h1 class="display-5">Authorization <span class="text-muted small">(Roles and Membership)</span></h1>
            </div>
            <div class="row">
                <div class="col-6 col-sm-4 col-md-3">
                    <div class="form-check form-check-inline">
                        <asp:CheckBox ID="CheckBoxRolePatient" Checked="true" CssClass="form-check-input mt-0 mr-1" runat="server" ClientIDMode="Static" />
                        <label class="form-check-label" for="<%= CheckBoxRolePatient.ClientID %>">Patient</label>
                    </div>
                </div>
                <div class="col-6 col-sm-4 col-md-3">
                    <div class="form-check form-check-inline">
                        <asp:CheckBox ID="CheckBoxRoleTherapist" CssClass="form-check-input mt-0 mr-1" runat="server" ClientIDMode="Static" />
                        <label class="form-check-label" for="<%= CheckBoxRoleTherapist.ClientID %>">Therapist</label>
                    </div>
                </div>
                <div class="col-6 col-sm-4 col-md-3">
                    <div class="form-check form-check-inline">
                        <asp:CheckBox ID="CheckBoxRoleResearcher" CssClass="form-check-input mt-0 mr-1" runat="server" ClientIDMode="Static" />
                        <label class="form-check-label" for="<%= CheckBoxRoleResearcher.ClientID %>">Researcher</label>
                    </div>
                </div>
                <div class="col-6 col-sm-4 col-md-3">
                    <div class="form-check form-check-inline">
                        <asp:CheckBox ID="CheckBoxRoleAdmin" CssClass="form-check-input mt-0 mr-1" runat="server" ClientIDMode="Static" />
                        <label class="form-check-label" for="<%= CheckBoxRoleAdmin.ClientID %>">Administrator</label>
                    </div>
                </div>
            </div>

            <div class="row mt-3">
                <div class="col-12 text-left">
                    <button type="submit" id="buttonRegister" class="btn btn-success mr-auto ml-auto" runat="server" onserverclick="ButtonRegister_ServerClick">Register</button>
                    <span id="spanMessage" class="small text-danger d-block d-sm-inline-block mt-2 mt-sm-0 ml-0 ml-sm-3" runat="server" visible="false"><i class="fas fa-exclamation-circle fa-fw"></i>There are errors in the form.</span>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanelRegistration" DisplayAfter="0" DynamicLayout="false">
        <ProgressTemplate>
            <div class="loading">Loading</div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div id="modelSuccess" class="modal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-dialog-centered text-center" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title mx-auto">Account has been Registered successfully</h5>
                </div>
                <div class="modal-body text-success">
                    <p class="mt-2"><i class="fas fa-check-circle fa-8x"></i></p>
                    <p class="display-3">success</p>
                </div>
                <div class="modal-footer">
                    <button type="submit" class="btn btn-success mr-auto" runat="server" onserverclick="buttonRefresh_ServerClick">Register another Account</button>
                    <a class="btn btn-secondary" href="~/Admin/Dashboard" role="button" runat="server">Return to Dashboard</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
