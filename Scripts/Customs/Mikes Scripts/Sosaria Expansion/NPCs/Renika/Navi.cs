using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    public class Navi : BaseCreature
    {
        [Constructable]
		public Navi() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Navi";
            Body = 0x190; // Human male body

            // Stats
            SetStr(100);
            SetDex(80);
            SetInt(110);
            SetHits(120);

            // Appearance: Robes of deep-sea blue with hints of crimson,
            // symbolic accessories such as a Tide Amulet and trinkets representing his controversial deity.
            AddItem(new Robe() { Hue = 0x455, Name = "Robe of the Undercurrents" });
            AddItem(new Sandals() { Hue = 0x455 });
            AddItem(new Necklace() { Name = "Tide Amulet" });
            AddItem(new GoldRing() { Name = "Sigil of the Abyss" });
            
            // Randomize skin hue and hair features
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
        }

        public Navi(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Greetings, weary traveler. I am Navi, tide priest and fervent servant of the Abyssal Mother—a deity both revered and reviled. The tides speak to me, and in their whispers lies an ancient, controversial truth. What wisdom do you seek on this fateful day?");

            // Option 1: Inquire about Navi's life and his controversial faith.
            greeting.AddOption("Tell me about your life as a tide priest and your faith.", 
                player => true,
                player =>
                {
                    DialogueModule lifeModule = new DialogueModule("My life is an endless cycle of devotion and revelation. I have spent countless years interpreting the roaring tides, and through them, I learned secrets that most dare not imagine. My faith in the Abyssal Mother demands zealous commitment—even if it means challenging orthodox beliefs. I have allied with Kess of Dawn and Syla of East Montor, though few know our true purpose.");
                    
                    lifeModule.AddOption("Reveal the true nature of the Abyssal Mother.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule deityModule = new DialogueModule("The Abyssal Mother is not merely a guardian of the ocean’s depths, but a force of primordial change—one that calls for transformation through sacrifice and devotion. My visions, which border on fanaticism, claim that the deity’s awakening will cleanse the old order. Yet, I must be discreet; such truths are met with both awe and outrage in equal measure. Would you dare to learn more?");
                            
                            deityModule.AddOption("Yes, I wish to know more about her sacred rites.",
                                plc => true,
                                plc =>
                                {
                                    DialogueModule ritesModule = new DialogueModule("In secret ceremonies beneath moonlit tides, I, along with a select few, invoke ancient chants to beckon the Abyssal Mother’s presence. The ritual involves forbidden texts, blood offerings, and incantations that even the devout find disturbing. It is an act of both desperation and hope—a transformative call for those willing to break the chains of tradition. Does this bold path stir your spirit?");
                                    
                                    ritesModule.AddOption("I am intrigued. Tell me the details of these rites.",
                                        plx => true,
                                        plx =>
                                        {
                                            DialogueModule detailsModule = new DialogueModule("Our rites commence at the stroke of midnight on the longest night of the year. We gather at a hidden cove where the sea meets ancient stone. Amid flickering torches and the rhythmic crashing of waves, we recite verses in a tongue lost to time. The culmination is a symbolic act: we cast a personal token into the churning sea, a pledge to embrace the Abyssal Mother’s merciless truth. Each token carries the weight of our past sins and future hopes.");
                                            plx.SendGump(new DialogueGump(plx, detailsModule));
                                        });
                                    ritesModule.AddOption("This ritual sounds dangerous… How have you not been discovered?",
                                        plx => true,
                                        plx =>
                                        {
                                            DialogueModule dangerModule = new DialogueModule("Danger is the constant companion of the zealous. Our gatherings are cloaked in secrecy, and only those proven worthy are granted entry. The coastal gods and local legends may speak of such madness, but true power is born from passion and sacrifice. Every whisper among the tides conceals our endeavors, and our unity shields us from prying eyes.");
                                            plx.SendGump(new DialogueGump(plx, dangerModule));
                                        });
                                    plc.SendGump(new DialogueGump(plc, ritesModule));
                                });
                            deityModule.AddOption("But isn't worshipping such a deity perilous?",
                                plc => true,
                                plc =>
                                {
                                    DialogueModule perilModule = new DialogueModule("Peril is the price of true enlightenment. The Abyssal Mother demands the shedding of old beliefs for new, radical truths. Many have balked at such audacity, yet I persist, for I see a future where the corrupt and stagnant are swept away by the tides of change. It is persuasive to those who have tasted the bitter fruit of conformity.");
                                    plc.SendGump(new DialogueGump(plc, perilModule));
                                });
                            pl.SendGump(new DialogueGump(pl, deityModule));
                        });
                    
                    lifeModule.AddOption("How do you relate to Kess, Syla, and Galven in your quest?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule relationsModule = new DialogueModule("Kess, with his gentle nature and innovative remedies, is the cornerstone of our coastal healing. Syla's insights grant clarity in tumultuous times, while Galven—the stalwart shipwright of Renika—shares in the mysteries that the deep holds. Yet, our alliances run deeper than simple camaraderie: we are bound by a shared, often controversial vision. I have even persuaded others to join our cause, though the whispers of our secret unite us in both passion and risk.");
                            
                            relationsModule.AddOption("Tell me more about Kess and his remedies.",
                                plc => true,
                                plc =>
                                {
                                    DialogueModule kessModule = new DialogueModule("Kess crafts his elixirs with more than skill—he infuses each potion with the essence of coastal magic. His methods are experimental, bordering on fanaticism himself at times. Many who have been healed by his remedies speak of an almost divine inspiration, though few know the extent of his fervor in our mysterious cause. Do you want to learn about a specific remedy he prepares?");
                                    
                                    kessModule.AddOption("Describe one of his most potent remedies.",
                                        plx => true,
                                        plx =>
                                        {
                                            DialogueModule remedyModule = new DialogueModule("One such concoction is the 'Elixir of the Tidal Dawn.' Crafted during the rare alignment of stars and tides, it is said to heal grievous wounds and unlock hidden potential within the soul. Its ingredients are gathered under strict secrecy, and its preparation is a ritual in itself. The elixir’s power is both a blessing and a dangerous allure.");
                                            plx.SendGump(new DialogueGump(plx, remedyModule));
                                        });
                                    kessModule.AddOption("Why do you value his work so highly?",
                                        plx => true,
                                        plx =>
                                        {
                                            DialogueModule valueModule = new DialogueModule("Kess’s remedies are more than mere medicine; they are a testament to our collective vision of renewal. Each potion represents a defiance of the conventional and a step toward a reborn coastal community, one that embraces the radical truth of our divine calling.");
                                            plx.SendGump(new DialogueGump(plx, valueModule));
                                        });
                                    plc.SendGump(new DialogueGump(plc, kessModule));
                                });
                            
                            relationsModule.AddOption("What does Syla contribute to your mission?",
                                plc => true,
                                plc =>
                                {
                                    DialogueModule sylaModule = new DialogueModule("Syla is our seer, a weaver of visions and a healer of spirit. Her meditations reveal portents that guide our actions, and her words often sway doubters into willing disciples. She understands the delicate balance between passion and peril, ensuring we do not stray too far into fanaticism. Would you like an example of her foresight?");
                                    
                                    sylaModule.AddOption("Yes, share one of her prophetic insights.",
                                        plx => true,
                                        plx =>
                                        {
                                            DialogueModule insightModule = new DialogueModule("Once, during a fierce storm, Syla foresaw the collapse of a coastal hamlet. Her warnings were dismissed as mere superstition—until the tide struck with unbridled fury. Her predictions saved lives and cemented her reputation as both a prophet and a persuasive force in our cause.");
                                            plx.SendGump(new DialogueGump(plx, insightModule));
                                        });
                                    sylaModule.AddOption("How does she balance her intuition with practical action?",
                                        plx => true,
                                        plx =>
                                        {
                                            DialogueModule balanceModule = new DialogueModule("Syla’s counsel is measured and precise. Even when the visions are dire, she advises calculated actions, integrating ancient rituals with modern survival tactics. Her blend of wisdom and pragmatism turns what many would call mere madness into a guiding light for those who seek transformation.");
                                            plx.SendGump(new DialogueGump(plx, balanceModule));
                                        });
                                    plc.SendGump(new DialogueGump(plc, sylaModule));
                                });
                            
                            relationsModule.AddOption("And what of Galven, your maritime partner?",
                                plc => true,
                                plc =>
                                {
                                    DialogueModule galvenModule = new DialogueModule("Galven is more than just a shipwright; he is a keeper of lost lore. His shipbuilding techniques are ancient, passed down through secret orders aligned with our deity. Together, we explore forgotten harbors and submerged ruins, seeking relics that testify to our cause. His practical genius balances my fervor, and his persuasion has often recruited skeptics to our radical vision.");
                                    
                                    galvenModule.AddOption("Tell me about one of your secret discoveries.",
                                        plx => true,
                                        plx =>
                                        {
                                            DialogueModule secretModule = new DialogueModule("Recently, while mapping the ruins of a sunken temple, Galven discovered a stone tablet etched with cryptic symbols. These inscriptions spoke of a 'Rising Tide'—a prophecy heralding the Abyssal Mother’s return. Such discoveries fortify our beliefs and compel us to act. It is a secret we keep close, for it has the power to sway even the most hardened hearts.");
                                            plx.SendGump(new DialogueGump(plx, secretModule));
                                        });
                                    galvenModule.AddOption("How does his work support your controversial faith?",
                                        plx => true,
                                        plx =>
                                        {
                                            DialogueModule supportModule = new DialogueModule("Galven’s craftsmanship channels the raw energy of the sea. He constructs vessels and altars imbued with mystical runes that resonate with our deity’s call. His work is a constant reminder that our mission transcends mortal limitations, inviting others to question the old ways and embrace the transformative power of the deep.");
                                            plx.SendGump(new DialogueGump(plx, supportModule));
                                        });
                                    plc.SendGump(new DialogueGump(plc, galvenModule));
                                });
                            pl.SendGump(new DialogueGump(pl, relationsModule));
                        });
                    
                    lifeModule.AddOption("What sacrifices have you made for your calling?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule sacrificeModule = new DialogueModule("My path has never been without sacrifice. I have given up family ties, renounced social order, and even risked my life to spread the word of the Abyssal Mother. These sacrifices are the crucible in which my faith was forged—a mix of zeal, passion, and an unyielding desire to see the old world washed away. Do you wish to hear of a particular sacrifice?");
                            
                            sacrificeModule.AddOption("Yes, recount one of your greatest sacrifices.",
                                plx => true,
                                plx =>
                                {
                                    DialogueModule recountModule = new DialogueModule("There was a time when I risked everything to save a coastal village from a catastrophic tempest. I led a clandestine ritual beneath raging skies, channeling all my resolve into a prayer that calmed the storm—at the cost of my own physical strength. Many whispered that I touched the very essence of the Abyssal Mother that night. It is a memory that both haunts and empowers me.");
                                    plx.SendGump(new DialogueGump(plx, recountModule));
                                });
                            sacrificeModule.AddOption("And what compelled you to make such sacrifices?",
                                plx => true,
                                plx =>
                                {
                                    DialogueModule compelModule = new DialogueModule("It is the fire of conviction, the unshakeable belief in a future reborn from chaos. I am driven by a purpose that transcends mortal restrictions. Every drop of sacrifice draws me closer to revealing the secret truth—the coming of the Abyssal Mother, who will redefine our destiny. Such is the nature of my fervor, and the cause it serves.");
                                    plx.SendGump(new DialogueGump(plx, compelModule));
                                });
                            pl.SendGump(new DialogueGump(pl, sacrificeModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, lifeModule));
                });

            // Option 2: Inquire about the mystical visions and the persuasive power of the tides.
            greeting.AddOption("What mystical visions does the tide reveal, and how do they guide you?", 
                player => true,
                player =>
                {
                    DialogueModule visionsModule = new DialogueModule("The tides unveil secrets that defy mortal reason. I have seen spectral fleets, dazzling lights beneath crushing depths, and omens that foretell both ruin and rebirth. Each vision is persuasive—a call to abandon the mundane and embrace the destiny ordained by the Abyssal Mother. Will you journey with me into these visions?");
                    
                    visionsModule.AddOption("Detail one of your most vivid visions.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule vividModule = new DialogueModule("I remember a vision bathed in the silver glow of a blood moon: a colossal sea serpent, scales shimmering like shattered glass, coiled around a ruined lighthouse. It whispered of a coming reckoning—a tidal wave of change that would cleanse the world of its sins. Its persuasive call stirred not only my heart but also those who listened. Does that chill your spine?");
                            
                            vividModule.AddOption("I feel its call. What must be done?",
                                plx => true,
                                plx =>
                                {
                                    DialogueModule callModule = new DialogueModule("The call is both a warning and an invitation. I urge worthy souls to cast aside their doubts and join our sacred ritual—where sacrifice, conviction, and unity will unlock the power of the deep. Only then can we truly usher in a new era, cleansed by the relentless tide of destiny.");
                                    plx.SendGump(new DialogueGump(plx, callModule));
                                });
                            vividModule.AddOption("That is too terrifying for me.",
                                plx => true,
                                plx =>
                                {
                                    DialogueModule fearModule = new DialogueModule("Fear is the natural reaction to the unknown. Yet, it is through facing such terror that one finds true strength. Understand that my visions, as zealous as they are, offer a path forward—one where even your fears might be transcended.");
                                    plx.SendGump(new DialogueGump(plx, fearModule));
                                });
                            pl.SendGump(new DialogueGump(pl, vividModule));
                        });
                    
                    visionsModule.AddOption("How do these visions transform you into a persuasive leader?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule leaderModule = new DialogueModule("Each vision imbues me with an urgency—a passion that compels others to listen. My words, fervent and unwavering, reflect the deep truths echoed by the ocean itself. My zeal is contagious, and many find themselves swayed by the promise of transformation. Shall I recount how this power has turned doubters into believers?");
                            
                            leaderModule.AddOption("Yes, share a tale of persuasion.",
                                plx => true,
                                plx =>
                                {
                                    DialogueModule persuasiveModule = new DialogueModule("There was a gathering of skeptical villagers on a stormy eve. With the wind howling like mournful spirits, I recited the ancient hymns passed down from the Abyssal Mother. Slowly, the fear in their eyes turned to fierce resolve as my words painted a vision of a world purified by the tide. That night, many embraced the call without hesitation, ready to renounce the old and welcome the new.");
                                    plx.SendGump(new DialogueGump(plx, persuasiveModule));
                                });
                            leaderModule.AddOption("I doubt such words could move me.",
                                plx => true,
                                plx =>
                                {
                                    DialogueModule doubtModule = new DialogueModule("Doubt is the seed of change—when questioned, it blossoms into understanding. Even if you do not succumb immediately, know that the tides will eventually reveal the truth to all who dare to listen. In time, the persuasive power of our purpose may yet draw you nearer.");
                                    plx.SendGump(new DialogueGump(plx, doubtModule));
                                });
                            pl.SendGump(new DialogueGump(pl, leaderModule));
                        });
                    
                    visionsModule.AddOption("Can these visions bring about real change in our world?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule changeModule = new DialogueModule("Absolutely. The visions are not mere fantasies; they are harbingers of a radical shift. I believe the Abyssal Mother will emerge to reshape our destiny—cleansing the corrupt and inspiring a rebirth among the faithful. Embracing these omens can liberate you from the shackles of outdated dogma. Would you like to hear of the changes already set in motion?");
                            
                            changeModule.AddOption("Yes, explain the changes.",
                                plx => true,
                                plx =>
                                {
                                    DialogueModule explainModule = new DialogueModule("In secret gatherings along the rugged coastline, whispers of transformation are already taking root. Families once resigned to hardship are now daring to dream of renewal. The very tide seems to promise that the old order will crumble, making way for a community built on this revolutionary faith. The signs are there for those who choose to see them.");
                                    plx.SendGump(new DialogueGump(plx, explainModule));
                                });
                            changeModule.AddOption("I remain unconvinced.",
                                plx => true,
                                plx =>
                                {
                                    DialogueModule unconvincedModule = new DialogueModule("Skepticism is the natural state of the uninitiated. Yet, I urge you to remain open—even a single glimpse of the truth may shatter your doubts. The sea does not lie; it calls to those willing to listen.");
                                    plx.SendGump(new DialogueGump(plx, unconvincedModule));
                                });
                            pl.SendGump(new DialogueGump(pl, changeModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, visionsModule));
                });

            // Option 3: Ask about his methods for recruiting followers to his mysterious cause.
            greeting.AddOption("How do you convince others to follow your controversial cause?", 
                player => true,
                player =>
                {
                    DialogueModule recruitModule = new DialogueModule("Ah, recruitment is an art—one that requires fervor, persuasion, and a willingness to embrace the unknown. I blend my visions with sincere compassion to show others that a radical change is possible. The Abyssal Mother’s call resonates deeply with those who feel trapped by convention.");
                    
                    recruitModule.AddOption("What persuasive methods do you use?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule methodsModule = new DialogueModule("I weave tales of ancient prophecies, recount personal sacrifices, and appeal to the innate desire for renewal. I challenge the status quo and offer a future where the oppressed are uplifted. My words are sharpened by my own zeal, converting cynics into crusaders almost by magic.");
                            
                            methodsModule.AddOption("Can you give an example of your persuasive rhetoric?",
                                plx => true,
                                plx =>
                                {
                                    DialogueModule exampleModule = new DialogueModule("At a forgotten pier during a tempest, I once addressed a gathering of disillusioned fishermen. I spoke of the tide as a harbinger of liberation—a force that would purge corruption and bestow fortune upon those brave enough to heed its call. The raw emotion in my speech stirred something primal in them, and by dawn, many had pledged themselves to our cause.");
                                    plx.SendGump(new DialogueGump(plx, exampleModule));
                                });
                            methodsModule.AddOption("What do you say to those who initially resist?",
                                plx => true,
                                plx =>
                                {
                                    DialogueModule resistanceModule = new DialogueModule("I remind the resistant that comfort in inaction breeds stagnation. I ask them if they are content with the suffering imposed by outdated hierarchies, or if they dare to dream of a dawn where even the tides rewrite destiny. My passion is infectious, and slowly, doubts dissolve into determination.");
                                    plx.SendGump(new DialogueGump(plx, resistanceModule));
                                });
                            pl.SendGump(new DialogueGump(pl, methodsModule));
                        });
                    
                    recruitModule.AddOption("What risks do you face in spreading your faith?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule riskModule = new DialogueModule("Danger lurks in every shadow for those who challenge the established order. My fervor has earned me both admiration and enmity. I have evaded the clutches of those who would silence our sacred truths, and my methods, though persuasive, are often seen as radical. The cost of change is high—but the promise of renewal justifies every sacrifice.");
                            
                            riskModule.AddOption("Describe one instance when you faced danger.",
                                plx => true,
                                plx =>
                                {
                                    DialogueModule dangerInstanceModule = new DialogueModule("There was an incident on a moonless night when men in dark cloaks attempted to seize our secret gathering. We scattered like the mist over the sea, leaving only echoes of our chants behind. I barely escaped, though the encounter only fortified my resolve to spread the Abyssal Mother's word. It was a close call that still haunts me—a reminder of the price of passion.");
                                    plx.SendGump(new DialogueGump(plx, dangerInstanceModule));
                                });
                            riskModule.AddOption("How do you overcome these dangers?",
                                plx => true,
                                plx =>
                                {
                                    DialogueModule overcomeModule = new DialogueModule("With vigilance, cunning, and an unyielding belief in my mission. I rely on trusted allies—Kess, Syla, and Galven—to protect our secrets and spread our message. Together, we form a network as fluid and unpredictable as the tide, ever ready to counter those who seek to halt our divine purpose.");
                                    plx.SendGump(new DialogueGump(plx, overcomeModule));
                                });
                            pl.SendGump(new DialogueGump(pl, riskModule));
                        });
                    
                    recruitModule.AddOption("What if I choose to join your cause?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule joinModule = new DialogueModule("Then you must be prepared to surrender the comfortable lies of the old world. Joining our cause means embracing uncertainty, upheaval, and the transformative power of the sea. It is a commitment not taken lightly—a bond sealed by sacrifice and faith. Are you ready to cast aside your doubts and pledge yourself to the Abyssal Mother?");
                            
                            joinModule.AddOption("Yes, I am ready to embrace your vision.",
                                plx => true,
                                plx =>
                                {
                                    DialogueModule pledgeModule = new DialogueModule("Your pledge is a covenant with destiny. In time, you will learn the sacred rites, partake in our rituals, and feel the persistent call of the tide. Welcome to a path where every sacrifice is a step toward a reborn world. Our first task will be a quiet ceremony on the shore at dawn—be prepared to leave your old self behind.");
                                    plx.SendGump(new DialogueGump(plx, pledgeModule));
                                });
                            joinModule.AddOption("I need more time to consider.",
                                plx => true,
                                plx =>
                                {
                                    DialogueModule reconsiderModule = new DialogueModule("Take the time you need, but remember: the tides wait for no one. The longer you resist, the deeper you may find yourself mired in complacency. Return when you are ready to heed the call of the deep and embrace a future unbound by tradition.");
                                    plx.SendGump(new DialogueGump(plx, reconsiderModule));
                                });
                            pl.SendGump(new DialogueGump(pl, joinModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, recruitModule));
                });
            
            // Option 4: For players curious about further secrets—more nested options with additional hints.
            greeting.AddOption("I sense that there is more hidden beneath your words. What else can you share?", 
                player => true,
                player =>
                {
                    DialogueModule secretModule = new DialogueModule("Ah, your intuition is commendable. There are depths even I have not fully plumbed. Beyond the tides and rituals, I harbor secret memories of a time when I was reborn through fire and storm. I whisper these secrets only to those I trust. Which forbidden truth do you wish to uncover?");
                    
                    secretModule.AddOption("Tell me about your hidden past.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule pastModule = new DialogueModule("Long ago, before I embraced the call of the Abyssal Mother, I was a simple priest of an orthodox pantheon. But a devastating vision shattered my world, revealing to me that the established order was but a veil—a corruption obscuring a higher truth. In that moment, I renounced all I had known and was consumed by a burning zeal. That secret, though dangerous, now fuels my every step.");
                            
                            pastModule.AddOption("How did that vision change you?",
                                plx => true,
                                plx =>
                                {
                                    DialogueModule visionChangeModule = new DialogueModule("It was as if the very essence of the ocean itself had been unleashed within me. I became an instrument of change—a fanatic, some say, driven by a passion that defies logic. I now see the world in stark contrasts: the impure clinging to the old and the shining promise of renewal beneath the tide's relentless surge.");
                                    plx.SendGump(new DialogueGump(plx, visionChangeModule));
                                });
                            pastModule.AddOption("Do you regret your past choices?",
                                plx => true,
                                plx =>
                                {
                                    DialogueModule regretModule = new DialogueModule("Regret is a luxury for those shackled by conventional morality. I do not regret the sacrifices nor the burning conviction that reshaped my destiny. Every choice, no matter how controversial, brought me closer to the liberating truth of the Abyssal Mother. In this revelation, there is no turning back—only forward into a promised dawn.");
                                    plx.SendGump(new DialogueGump(plx, regretModule));
                                });
                            pl.SendGump(new DialogueGump(pl, pastModule));
                        });
                    
                    secretModule.AddOption("What dangerous truths do you keep hidden?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule dangerSecretModule = new DialogueModule("Some truths are so potent that they could upheave entire realms. I guard knowledge of ancient pacts made in blood and salt—the covenant of the Abyssal Mother that binds the fate of the coast. These secrets, whispered on stormy nights, hint at a coming purge of the old order, and while such power is alluring, it carries with it unspeakable consequences.");
                            
                            dangerSecretModule.AddOption("How might these secrets affect our world?",
                                plx => true,
                                plx =>
                                {
                                    DialogueModule effectModule = new DialogueModule("Should these secrets come to light, they would ignite upheaval. The meek would rise in rebellion, and the mighty would tremble in fear. My mission is not just prophecy—it is a call to rebuild from the ashes of a decaying system. Change is wrought by those brave enough to embrace the dangerous unknown.");
                                    plx.SendGump(new DialogueGump(plx, effectModule));
                                });
                            dangerSecretModule.AddOption("Is there a way to safely learn these truths?",
                                plx => true,
                                plx =>
                                {
                                    DialogueModule safeModule = new DialogueModule("Truth, by its very nature, is seldom safe. It demands sacrifice, resilience, and above all, unwavering faith. Those who seek it must first prove their worthiness—a test of both heart and will. Perhaps in time, if the tide deems you ready, I will share more of these forbidden revelations.");
                                    plx.SendGump(new DialogueGump(plx, safeModule));
                                });
                            pl.SendGump(new DialogueGump(pl, dangerSecretModule));
                        });
                    
                    secretModule.AddOption("I wish to keep these secrets to myself for now.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule leaveSecretModule = new DialogueModule("Very well. The tides hold many secrets; one is free for the taking, while the rest remain locked away until the destined moment. Return when your spirit is ready to dive deeper into the shadows.");
                            pl.SendGump(new DialogueGump(pl, leaveSecretModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, secretModule));
                });

            return greeting;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
