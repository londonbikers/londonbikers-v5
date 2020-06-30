using System;
using System.Text.RegularExpressions;

namespace Apollo.Utilities
{
    /// <summary>
    /// Provides common utilities for textual operations.
    /// </summary>
    public class Text
    {
        public static string ToXmlString(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            text = text.Replace("&amp;", "&");
            text = text.Replace("&", "&amp;");
            return text;
        }

        /// <summary>
        /// Converts many common US spellings to UK ones, i.e. color to colour.
        /// </summary>
        public static string ConvertUsSpellingToUk(string textToTranslate)
        {
            #region word translations
            // not very efficient, but time does not allow for anything nicer.

            // singulars
            textToTranslate = Regex.Replace(textToTranslate, @"\barbor\b", "arbour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bardor\b", "ardour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\barmor\b", "armour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\barmory\b", "armoury", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bbehavior\b", "behaviour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bbehavioral\b", "behavioural", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bbehaviorism\b", "behaviourism", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bbelabor\b", "belabour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcandor\b", "candour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bclamor\b", "clamour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcolor\b", "colour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcoloration\b", "coloration", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bdemeanor\b", "demeanour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\benamor\b", "enamour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bendeavor\b", "endeavour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bfavor\b", "favour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bfavorable\b", "favourable", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bfavorite\b", "favourite", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bfavoritism\b", "favouritism", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bfervor\b", "fervour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bflavor\b", "flavour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bglamor\b", "glamour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bharbor\b", "harbour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bhonor\b", "honour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bhonorable\b", "honourable", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bhumor\b", "humour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\blabor\b", "labour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmisdemeanor\b", "misdemeanour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bneighbor\b", "neighbour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bodor\b", "odour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bparlor\b", "parlour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\brancor\b", "rancour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\brigor\b", "rigour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\brumor\b", "rumour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bsavior\b", "saviour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bsavor\b", "savour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bsplendor\b", "splendour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bsuccor\b", "succour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\btumor\b", "tumour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bvalor\b", "valour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bvapor\b", "vapour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bvigor\b", "vigour", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcaliber\b", "calibre", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcentiliter\b", "centilitre", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcentimeter\b", "centimetre", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcenter\b", "centre", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcentered\b", "centred", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcentering\b", "centring", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bdeciliter\b", "decilitre", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bdecimeter\b", "decimetre", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bepicenter\b", "epicentre", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bgoiter\b", "goitre", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bkilometer\b", "kilometre", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\blackluster\b", "lacklustre", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bliter\b", "litre", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bluster\b", "lustre", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmeager\b", "meagre", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmeter\b", "metre", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmicrometer\b", "micrometre", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmilliliter\b", "millilitre", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmillimeter\b", "millimetre", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmiter\b", "mitre", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmitered\b", "mitred", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bphilter\b", "philtre", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\breconnoiter\b", "reconnoitre", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\breconnoitered\b", "reconnoitred", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\breconnoitering\b", "reconnoitring", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bsaber\b", "sabre", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bsaltpeter\b", "saltpetre", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bscepter\b", "sceptre", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bsceptered\b", "sceptred", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcounselor\b", "counsellor", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bequaled\b", "equalled", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bequaling\b", "equalling", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bdialer\b", "dialler", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\benroll\b", "enrol", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\benrollment\b", "enrolment", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bfulfill\b", "fulfil", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bfulfillment\b", "fulfilment", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\binstallment\b", "instalment", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bskillful\b", "skilful", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bwillful\b", "wilful", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\banalogue\b", "analogue", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcatalog\b", "catalogue", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bdialogue\b", "dialogue", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmonologue\b", "monologue", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bapologize\b", "apologise", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bemphasize\b", "emphasise", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcolonize\b", "colonise", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bglobalization\b", "globalisation", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmaximize\b", "maximise", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\borganization\b", "organisation", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bprivatization\b", "privatisation", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\brealize\b", "realise", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\brecognize\b", "recognise", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\brecognizable\b", "recognisable", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bstandardize\b", "standardise", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\banalyze\b", "analyse", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\banalyzing\b", "analysing", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bbreathalyzer\b", "breathalyser", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcatalyze\b", "catalyse", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcatalyzing\b", "catalysing", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bdialyzer\b", "dialyser", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bhydrolyze\b", "hydrolyse", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bhydrolyzing\b", "hydrolysing", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bparalyze\b", "paralyse", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bparalyzing\b", "paralysing", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bphotolyze\b", "photolyse", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bphotolyzing\b", "photolysing", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bdefense\b", "defence", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\boffense\b", "offence", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bpretense\b", "pretence", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\banemia\b", "anaemia", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\banesthesia\b", "anaesthesia", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcesium\b", "caesium", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bdiarrhea\b", "diarrhoea", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bfeces\b", "faeces", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bfetus\b", "foetus", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bgonorrhea\b", "gonorrhoea", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bgynecology\b", "gynaecology", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bhemoglobin\b", "haemoglobin", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bhemophilia\b", "haemophilia", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bhemorrhage\b", "haemorrhage", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bischemic\b", "ischaemic", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bleukemia\b", "leukaemia", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmaneuver\b", "manoeuvre", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmaneuvering\b", "manoeuvring", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bedema\b", "oedema", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\besophagus\b", "oesophagus", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bestrogen\b", "oestrogen", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\borthopedic\b", "orthopaedic", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bpediatrics\b", "paediatrics", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bpedophile\b", "paedophile", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bpaleontology\b", "palaeontology", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\baging\b", "ageing", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\baluminum\b", "aluminium", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcarburetor\b", "carburettor", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bdreamed\b", "dreamt", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bgray\b", "grey", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bleaped\b", "leapt", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmath\b", "maths", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmold\b", "mould", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmoldering\b", "mouldering", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmolding\b", "moulding", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmoldy\b", "mouldy", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmolt\b", "moult", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmustache\b", "moustache", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmom\b", "mum", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bprogram\b", "programme", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bpajamas\b", "pyjamas", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bkeptical\b", "sceptical", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bspelled\b", "spelt", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bstoried\b", "storeyed", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bsulfur\b", "sulphur", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\btire\b", "tyre", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            // plurals. God I wish I could think of a quick and easy way to do this properly.
            textToTranslate = Regex.Replace(textToTranslate, @"\barbors\b", "arbours", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bardors\b", "ardours", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\barmories\b", "armouries", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bbehaviors\b", "behaviours", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bclamors\b", "clamours", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcolors\b", "colours", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcolorations\b", "colorations", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bendeavors\b", "endeavours", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bfavors\b", "favours", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bfavorites\b", "favourites", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bfervors\b", "fervours", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bflavors\b", "flavours", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bharbors\b", "harbours", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bhonors\b", "honours", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bhonorables\b", "honourables", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bhumors\b", "humours", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\blabors\b", "labours", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmisdemeanors\b", "misdemeanours", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bneighbors\b", "neighbours", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bodors\b", "odours", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bparlors\b", "parlours", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\brancors\b", "rancours", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\brigors\b", "rigours", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\brumors\b", "rumours", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bsaviors\b", "saviours", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bsavors\b", "savours", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bsplendors\b", "splendours", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\btumors\b", "tumours", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bvapors\b", "vapours", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcalibers\b", "calibres", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcentimeters\b", "centimetres", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcenters\b", "centres", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcenterings\b", "centrings", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bkilometers\b", "kilometres", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bliters\b", "litres", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmeters\b", "metres", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmilliliters\b", "millilitres", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmillimeters\b", "millimetres", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmiters\b", "mitres", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\breconnoiterings\b", "reconnoitrings", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bsabers\b", "sabres", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bscepters\b", "sceptres", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcounselors\b", "counsellors", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bequalings\b", "equallings", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bdialers\b", "diallers", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\benrolls\b", "enrols", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\benrollments\b", "enrolments", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bfulfills\b", "fulfils", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\binstallments\b", "instalments", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\banalogues\b", "analogues", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcatalogs\b", "catalogues", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bdialogues\b", "dialogues", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmonologues\b", "monologues", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bapologizes\b", "apologises", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bemphasizes\b", "emphasises", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcolonizes\b", "colonises", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmaximizes\b", "maximises", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\borganizations\b", "organisations", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bprivatizations\b", "privatisations", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\brealizes\b", "realises", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\brecognizes\b", "recognises", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\brecognizables\b", "recognisables", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bstandardizes\b", "standardises", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\banalyzes\b", "analyses", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bbreathalyzers\b", "breathalysers", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcatalyzes\b", "catalyses", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bdialyzers\b", "dialysers", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bhydrolyzes\b", "hydrolyses", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bhydrolyzings\b", "hydrolysings", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bparalyzes\b", "paralyses", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bparalyzings\b", "paralysings", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bphotolyzes\b", "photolyses", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bphotolyzings\b", "photolysings", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bdefenses\b", "defences", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\boffenses\b", "offences", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\banemias\b", "anaemias", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\banesthesias\b", "anaesthesias", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcesiums\b", "caesiums", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bdiarrhesa\b", "diarrhoeas", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bfeces\b", "faeces", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bfetus\b", "foetus", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bgonorrheas\b", "gonorrhoeas", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bgynecology\b", "gynaecology", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bhemoglobins\b", "haemoglobins", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bhemophilias\b", "haemophilias", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bhemorrhages\b", "haemorrhages", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmaneuvers\b", "manoeuvres", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmaneuverings\b", "manoeuvrings", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bedemas\b", "oedemas", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bestrogens\b", "oestrogens", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\borthopedics\b", "orthopaedics", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bpedophiles\b", "paedophiles", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bagings\b", "ageings", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\baluminums\b", "aluminiums", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bcarburetors\b", "carburettors", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bgrays\b", "greys", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmolds\b", "moulds", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmolderings\b", "moulderings", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmoldings\b", "mouldings", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmolts\b", "moults", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmustaches\b", "moustaches", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bmoms\b", "mums", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bprograms\b", "programmes", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bkepticals\b", "scepticals", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\bsulfurs\b", "sulphurs", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            textToTranslate = Regex.Replace(textToTranslate, @"\btires\b", "tyres", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            #endregion

            return textToTranslate;
        }

        /// <summary>
        /// Provides a fast and logical (case-retaining) way to replace strings within one-another.
        /// </summary>
        /// <param name="original">The original text to replace parts of.</param>
        /// <param name="pattern">The Regular Expression pattern.</param>
        /// <param name="replacement">The text to have the matching text replaced with in each instance.</param>
        public static string ReplaceEx(string original, string pattern, string replacement)
        {
            int position0, position1;
            var count = position0 = 0;
            var upperString = original.ToUpper();
            var upperPattern = pattern.ToUpper();
            var inc = (original.Length / pattern.Length) * (replacement.Length - pattern.Length);
            var chars = new char[original.Length + Math.Max(0, inc)];

            while ((position1 = upperString.IndexOf(upperPattern, position0)) != -1)
            {
                for (var i = position0; i < position1; ++i)
                    chars[count++] = original[i];

                foreach (var t in replacement)
                    chars[count++] = t;

                position0 = position1 + pattern.Length;
            }
            
            if (position0 == 0) 
                return original;

            for (var i = position0; i < original.Length; ++i)
                chars[count++] = original[i];

            return new string(chars, 0, count);
        }
    }
}