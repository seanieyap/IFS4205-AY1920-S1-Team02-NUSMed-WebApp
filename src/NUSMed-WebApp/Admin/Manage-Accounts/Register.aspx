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
                        <input id="inputNRIC" type="text" class="form-control" placeholder="NRIC" runat="server">
                        <div class="invalid-feedback" runat="server">
                            NRIC is invalid or has already been registered.
                        </div>
                    </div>
                </div>
                <div class="col-12 col-md-6">
                    <div class="form-group">
                        <label for="inputDoB">Date of Birth</label>
                        <input id="inputDoB" name="dateOfBirth" type="date" class="form-control" placeholder="Date of Birth" runat="server">
                        <div class="invalid-feedback" runat="server">
                            Date of Birth is invalid.
                        </div>
                    </div>
                </div>
                <div class="col-12 col-md-6">
                    <div class="form-group">
                        <label for="inputFirstName">First Name</label>
                        <input id="inputFirstName" type="text" class="form-control" placeholder="First Name" runat="server">
                        <div class="invalid-feedback" runat="server">
                            First Name is invalid.
                        </div>
                    </div>
                </div>
                <div class="col-12 col-md-6">
                    <div class="form-group">
                        <label for="inputLastName">Last Name</label>
                        <input id="inputLastName" type="text" class="form-control" placeholder="Last Name" runat="server">
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
                            <option value="Afghan">Afghan </option>
                            <option value="Albanian">Albanian </option>
                            <option value="Algerian">Algerian </option>
                            <option value="American">American </option>
                            <option value="Andorran">Andorran </option>
                            <option value="Angolan">Angolan </option>
                            <option value="Antiguans">Antiguans </option>
                            <option value="Argentinean">Argentinean </option>
                            <option value="Armenian">Armenian </option>
                            <option value="Australian">Australian </option>
                            <option value="Austrian">Austrian </option>
                            <option value="Azerbaijani">Azerbaijani </option>
                            <option value="Bahamian">Bahamian </option>
                            <option value="Bahraini">Bahraini </option>
                            <option value="Bangladeshi">Bangladeshi </option>
                            <option value="Barbadian">Barbadian </option>
                            <option value="Barbudans">Barbudans </option>
                            <option value="Batswana">Batswana </option>
                            <option value="Belarusian">Belarusian </option>
                            <option value="Belgian">Belgian </option>
                            <option value="Belizean">Belizean </option>
                            <option value="Beninese">Beninese </option>
                            <option value="Bhutanese">Bhutanese </option>
                            <option value="Bolivian">Bolivian </option>
                            <option value="Bosnian">Bosnian </option>
                            <option value="Brazilian">Brazilian </option>
                            <option value="British">British </option>
                            <option value="Bruneian">Bruneian </option>
                            <option value="Bulgarian">Bulgarian </option>
                            <option value="Burkinabe">Burkinabe </option>
                            <option value="Burmese">Burmese </option>
                            <option value="Burundian">Burundian </option>
                            <option value="Cambodian">Cambodian </option>
                            <option value="Cameroonian">Cameroonian </option>
                            <option value="Canadian">Canadian </option>
                            <option value="Cape Verdean">Cape Verdean </option>
                            <option value="Central African">Central African </option>
                            <option value="Chadian">Chadian </option>
                            <option value="Chilean">Chilean </option>
                            <option value="Chinese">Chinese </option>
                            <option value="Colombian">Colombian </option>
                            <option value="Comoran">Comoran </option>
                            <option value="Congolese">Congolese </option>
                            <option value="Congolese">Congolese </option>
                            <option value="Costa Rican">Costa Rican </option>
                            <option value="Croatian">Croatian </option>
                            <option value="Cuban">Cuban </option>
                            <option value="Cypriot">Cypriot </option>
                            <option value="Czech">Czech </option>
                            <option value="Danish">Danish </option>
                            <option value="Djibouti">Djibouti </option>
                            <option value="Dominican">Dominican </option>
                            <option value="Dominican">Dominican </option>
                            <option value="Dutch">Dutch </option>
                            <option value="Dutchman">Dutchman </option>
                            <option value="Dutchwoman">Dutchwoman </option>
                            <option value="East Timorese">East Timorese </option>
                            <option value="Ecuadorean">Ecuadorean </option>
                            <option value="Egyptian">Egyptian </option>
                            <option value="Emirian">Emirian </option>
                            <option value="Equatorial Guinean">Equatorial Guinean </option>
                            <option value="Eritrean">Eritrean </option>
                            <option value="Estonian">Estonian </option>
                            <option value="Ethiopian">Ethiopian </option>
                            <option value="Fijian">Fijian </option>
                            <option value="Filipino">Filipino </option>
                            <option value="Finnish">Finnish </option>
                            <option value="French">French </option>
                            <option value="Gabonese">Gabonese </option>
                            <option value="Gambian">Gambian </option>
                            <option value="Georgian">Georgian </option>
                            <option value="German">German </option>
                            <option value="Ghanaian">Ghanaian </option>
                            <option value="Greek">Greek </option>
                            <option value="Grenadian">Grenadian </option>
                            <option value="Guatemalan">Guatemalan </option>
                            <option value="Guinea-Bissauan">Guinea-Bissauan </option>
                            <option value="Guinean">Guinean </option>
                            <option value="Guyanese">Guyanese </option>
                            <option value="Haitian">Haitian </option>
                            <option value="Herzegovinian">Herzegovinian </option>
                            <option value="Honduran">Honduran </option>
                            <option value="Hungarian">Hungarian </option>
                            <option value="I-Kiribati">I-Kiribati </option>
                            <option value="Icelander">Icelander </option>
                            <option value="Indian">Indian </option>
                            <option value="Indonesian">Indonesian </option>
                            <option value="Iranian">Iranian </option>
                            <option value="Iraqi">Iraqi </option>
                            <option value="Irish">Irish </option>
                            <option value="Irish">Irish </option>
                            <option value="Israeli">Israeli </option>
                            <option value="Italian">Italian </option>
                            <option value="Ivorian">Ivorian </option>
                            <option value="Jamaican">Jamaican </option>
                            <option value="Japanese">Japanese </option>
                            <option value="Jordanian">Jordanian </option>
                            <option value="Kazakhstani">Kazakhstani </option>
                            <option value="Kenyan">Kenyan </option>
                            <option value="Kittian and Nevisian">Kittian and Nevisian </option>
                            <option value="Kuwaiti">Kuwaiti </option>
                            <option value="Kyrgyz">Kyrgyz </option>
                            <option value="Laotian">Laotian </option>
                            <option value="Latvian">Latvian </option>
                            <option value="Lebanese">Lebanese </option>
                            <option value="Liberian">Liberian </option>
                            <option value="Libyan">Libyan </option>
                            <option value="Liechtensteiner">Liechtensteiner </option>
                            <option value="Lithuanian">Lithuanian </option>
                            <option value="Luxembourger">Luxembourger </option>
                            <option value="Macedonian">Macedonian </option>
                            <option value="Malagasy">Malagasy </option>
                            <option value="Malawian">Malawian </option>
                            <option value="Malaysian">Malaysian </option>
                            <option value="Maldivan">Maldivan </option>
                            <option value="Malian">Malian </option>
                            <option value="Maltese">Maltese </option>
                            <option value="Marshallese">Marshallese </option>
                            <option value="Mauritanian">Mauritanian </option>
                            <option value="Mauritian">Mauritian </option>
                            <option value="Mexican">Mexican </option>
                            <option value="Micronesian">Micronesian </option>
                            <option value="Moldovan">Moldovan </option>
                            <option value="Monacan">Monacan </option>
                            <option value="Mongolian">Mongolian </option>
                            <option value="Moroccan">Moroccan </option>
                            <option value="Mosotho">Mosotho </option>
                            <option value="Motswana">Motswana </option>
                            <option value="Mozambican">Mozambican </option>
                            <option value="Namibian">Namibian </option>
                            <option value="Nauruan">Nauruan </option>
                            <option value="Nepalese">Nepalese </option>
                            <option value="Netherlander">Netherlander </option>
                            <option value="New Zealander">New Zealander </option>
                            <option value="Ni-Vanuatu">Ni-Vanuatu </option>
                            <option value="Nicaraguan">Nicaraguan </option>
                            <option value="Nigerian">Nigerian </option>
                            <option value="Nigerien">Nigerien </option>
                            <option value="North Korean">North Korean </option>
                            <option value="Northern Irish">Northern Irish </option>
                            <option value="Norwegian">Norwegian </option>
                            <option value="Omani">Omani </option>
                            <option value="Pakistani">Pakistani </option>
                            <option value="Palauan">Palauan </option>
                            <option value="Panamanian">Panamanian </option>
                            <option value="Papua New Guinean">Papua New Guinean </option>
                            <option value="Paraguayan">Paraguayan </option>
                            <option value="Peruvian">Peruvian </option>
                            <option value="Polish">Polish </option>
                            <option value="Portuguese">Portuguese </option>
                            <option value="Qatari">Qatari </option>
                            <option value="Romanian">Romanian </option>
                            <option value="Russian">Russian </option>
                            <option value="Rwandan">Rwandan </option>
                            <option value="Saint Lucian">Saint Lucian </option>
                            <option value="Salvadoran">Salvadoran </option>
                            <option value="Samoan">Samoan </option>
                            <option value="San Marinese">San Marinese </option>
                            <option value="Sao Tomean">Sao Tomean </option>
                            <option value="Saudi">Saudi </option>
                            <option value="Scottish">Scottish </option>
                            <option value="Senegalese">Senegalese </option>
                            <option value="Serbian">Serbian </option>
                            <option value="Seychellois">Seychellois </option>
                            <option value="Sierra Leonean">Sierra Leonean </option>
                            <option value="Singaporean">Singaporean </option>
                            <option value="Slovakian">Slovakian </option>
                            <option value="Slovenian">Slovenian </option>
                            <option value="Solomon Islander">Solomon Islander </option>
                            <option value="Somali">Somali </option>
                            <option value="South African">South African </option>
                            <option value="South Korean">South Korean </option>
                            <option value="Spanish">Spanish </option>
                            <option value="Sri Lankan">Sri Lankan </option>
                            <option value="Sudanese">Sudanese </option>
                            <option value="Surinamer">Surinamer </option>
                            <option value="Swazi">Swazi </option>
                            <option value="Swedish">Swedish </option>
                            <option value="Swiss">Swiss </option>
                            <option value="Syrian">Syrian </option>
                            <option value="Taiwanese">Taiwanese </option>
                            <option value="Tajik">Tajik </option>
                            <option value="Tanzanian">Tanzanian </option>
                            <option value="Thai">Thai </option>
                            <option value="Togolese">Togolese </option>
                            <option value="Tongan">Tongan </option>
                            <option value="Trinidadian or Tobagonian">Trinidadian or Tobagonian </option>
                            <option value="Tunisian">Tunisian </option>
                            <option value="Turkish">Turkish </option>
                            <option value="Tuvaluan">Tuvaluan </option>
                            <option value="Ugandan">Ugandan </option>
                            <option value="Ukrainian">Ukrainian </option>
                            <option value="Uruguayan">Uruguayan </option>
                            <option value="Uzbekistani">Uzbekistani </option>
                            <option value="Venezuelan">Venezuelan </option>
                            <option value="Vietnamese">Vietnamese </option>
                            <option value="Welsh">Welsh </option>
                            <option value="Welsh">Welsh </option>
                            <option value="Yemenite">Yemenite </option>
                            <option value="Zambian">Zambian </option>
                            <option value="Zimbabwean">Zimbabwean </option>
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
                            <asp:RadioButton ID="RadioButtonGenderTrans" CssClass="form-check-input" runat="server" GroupName="RadioButtonGender" />
                            <label class="form-check-label" for="<%= RadioButtonGenderTrans.ClientID %>">Trans</label>
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
                        <label>Marital Status</label>
                        <br />
                        <div class="form-check form-check-inline">
                            <asp:RadioButton ID="RadioButtonMaritalStatusSingle" CssClass="form-check-input" runat="server" GroupName="RadioButtonMaritalStatus" Checked="true" />
                            <label class="form-check-label" for="<%= RadioButtonMaritalStatusSingle.ClientID %>">Single</label>
                        </div>
                        <div class="form-check form-check-inline">
                            <asp:RadioButton ID="RadioButtonMaritalStatusMarried" CssClass="form-check-input" runat="server" GroupName="RadioButtonMaritalStatus" />
                            <label class="form-check-label" for="<%= RadioButtonMaritalStatusMarried.ClientID %>">Married</label>
                        </div>
                        <div class="form-check form-check-inline">
                            <asp:RadioButton ID="RadioButtonMaritalStatusDivorced" CssClass="form-check-input" runat="server" GroupName="RadioButtonMaritalStatus" />
                            <label class="form-check-label" for="<%= RadioButtonMaritalStatusDivorced.ClientID %>">Divorced</label>
                        </div>
                        <div class="form-check form-check-inline">
                            <asp:RadioButton ID="RadioButtonMaritalStatusWidowed" CssClass="form-check-input" runat="server" GroupName="RadioButtonMaritalStatus" />
                            <label class="form-check-label" for="<%= RadioButtonMaritalStatusWidowed.ClientID %>">Widowed</label>
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
                        <input id="inputAddress" type="text" class="form-control" placeholder="Address" runat="server">
                        <div class="invalid-feedback" runat="server">
                            Address is invalid.
                        </div>
                    </div>
                </div>
                <div class="col-12 col-lg-4">
                    <div class="form-group">
                        <label for="inputPostalCode">Postal Code <span class="text-muted small">(6-digits)</span></label>
                        <input id="inputPostalCode" type="text" class="form-control" placeholder="Postal Code" runat="server">
                        <div class="invalid-feedback" runat="server">
                            Postal Code is invalid.
                        </div>
                    </div>
                </div>
                <div class="col-12 col-lg-8">
                    <div class="form-group">
                        <label for="inputEmail">Email Address</label>
                        <input id="inputEmail" type="email" class="form-control" placeholder="Email Address" runat="server">
                        <div class="invalid-feedback" runat="server">
                            Email Address is invalid.
                        </div>
                    </div>
                </div>
                <div class="col-12 col-lg-4">
                    <div class="form-group">
                        <label for="inputContactNumber">Contact Number <span class="text-muted small">(8-digits)</span></label>
                        <input id="inputContactNumber" type="tel" class="form-control" placeholder="Contact Number" runat="server">
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
