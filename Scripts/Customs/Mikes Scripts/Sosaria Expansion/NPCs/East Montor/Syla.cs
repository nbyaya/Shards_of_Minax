using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    public class Syla : BaseCreature
    {
        [Constructable]
        public Syla() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Syla";
            Body = 0x190; // Human body type

            // Set core stats
            SetStr(90);
            SetDex(80);
            SetInt(150);
            SetHits(100);

            // Appearance and equipment
            AddItem(new Robe() { Hue = 1750, Name = "Mystic Healer's Robe" });
            AddItem(new Cap() { Hue = 2150, Name = "Healer's Cap" });
            AddItem(new Sandals() { Hue = 1650 });
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
        }

        public Syla(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;
            
            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            // Main greeting: Introduce Syla, her healing expertise, and hint at a mysterious, fanatical past.
            DialogueModule greeting = new DialogueModule(
                "Greetings, traveler. I am Syla—renowned as a healer and seeker of ancient truth. " +
                "I serve those in need and work with Kess of Dawn to cure emerging illnesses, " +
                "with Jonas of West Montor to unlock arcane secrets, and with Torren in restoring our lost sanctuaries. " +
                "Yet behind my public duty lies a fervent calling... a divine mandate entrusted to me by a controversial deity. " +
                "Tell me, what knowledge do you seek today?"
            );

            // Option 1: Healing Traditions and Secret Rituals
            greeting.AddOption("Tell me about your healing traditions and secret rites.", 
                player => true,
                player =>
                {
                    DialogueModule healingModule = new DialogueModule(
                        "I have long studied the sacred arts passed down in Renika. " +
                        "My healing practices blend natural herbal lore with ancient incantations. " +
                        "But there is more—a secret ritual, known only to the chosen, that channels the divine essence of the Shrouded Moon, " +
                        "the deity who revealed my destiny. Do you wish to explore my herbal remedies, my ritual techniques, or the sacred rites of the Shrouded Moon?"
                    );

                    // Branch: Herbal Remedies
                    healingModule.AddOption("Explain your herbal remedies in detail.",
                        p => true,
                        p =>
                        {
                            DialogueModule herbalModule = new DialogueModule(
                                "In the sacred groves of Renika, I harvest Silverleaf, Moonwort, and Nightbloom under the watchful glow of twilight. " +
                                "These herbs are combined with spring water and a whisper of incantation: 'Luminara, bless these leaves, " +
                                "imbue them with life and healing.' Would you like to know the precise sequence of this ritual, or the hidden symbolism behind each herb?"
                            );
                            herbalModule.AddOption("Detail the sequence of the herbal ritual.",
                                q => true,
                                q =>
                                {
                                    DialogueModule sequenceModule = new DialogueModule(
                                        "First, the herbs are gathered at dusk. Next, they are ground into a fine paste while I chant: 'Luminara, reveal thy power.' " +
                                        "Then, I mix in the clear spring water and leave the elixir to steep under the full moon. " +
                                        "This process not only purifies the ingredients but also aligns them with the cosmic energies. " +
                                        "Are you intrigued by the mystic timing or by the transformative power of the full moon?"
                                    );
                                    sequenceModule.AddOption("Tell me about the full moon's role.",
                                        r => true,
                                        r =>
                                        {
                                            DialogueModule moonModule = new DialogueModule(
                                                "The full moon is a harbinger of transformation, a time when the veil between our world and the divine thins. " +
                                                "Its silver light infuses the remedy with potent rejuvenation. " +
                                                "Some even say that its beams awaken hidden powers within the soul itself."
                                            );
                                            r.SendGump(new DialogueGump(r, moonModule));
                                        });
                                    sequenceModule.AddOption("I am fascinated by mystic timing.",
                                        r => true,
                                        r =>
                                        {
                                            DialogueModule timingModule = new DialogueModule(
                                                "Each moment of the ritual is meticulously planned—the gathering at twilight, the grinding beneath starlight, " +
                                                "and the final steeping under the full moon create a symphony of nature and magic. " +
                                                "It is a dance between the mundane and the divine."
                                            );
                                            r.SendGump(new DialogueGump(r, timingModule));
                                        });
                                    q.SendGump(new DialogueGump(q, sequenceModule));
                                });
                            herbalModule.AddOption("Explain the symbolism behind the herbs.",
                                q => true,
                                q =>
                                {
                                    DialogueModule symbolismModule = new DialogueModule(
                                        "Each herb carries its own secret: Silverleaf reflects purity and the light of hope, " +
                                        "Moonwort embodies the mysteries of the night, and Nightbloom speaks to the hidden truths of sorrow and recovery. " +
                                        "By merging their essences, the remedy becomes a microcosm of life's duality—its pain and its healing."
                                    );
                                    q.SendGump(new DialogueGump(q, symbolismModule));
                                });
                            p.SendGump(new DialogueGump(p, herbalModule));
                        });

                    // Branch: Ritual Techniques
                    healingModule.AddOption("Elaborate on your ritual healing techniques.",
                        p => true,
                        p =>
                        {
                            DialogueModule ritualModule = new DialogueModule(
                                "My ritual healing is an art—a sacred performance. " +
                                "When performing such rites, I create a circle of light with carefully arranged runes and incense. " +
                                "I recite ancient verses passed down by my mentor, and each gesture is imbued with intention. " +
                                "During times of crisis, like the recent outbreak where my rituals combined with Kess's cures, " +
                                "the divine energy flowed so powerfully that it seemed the very heavens wept tears of relief. " +
                                "Would you like to hear about a specific ritual or the symbolism behind each element of the performance?"
                            );
                            ritualModule.AddOption("Recount a specific healing ritual.",
                                q => true,
                                q =>
                                {
                                    DialogueModule ritualDetailModule = new DialogueModule(
                                        "Once, amid a severe fever epidemic, I led a healing circle by a roaring bonfire. " +
                                        "I positioned sacred runes in a pattern that echoed the celestial alignment of the Shrouded Moon, " +
                                        "chanting fervently as embers danced in the night. In that moment, every heart present felt the spark " +
                                        "of divine intervention—a miracle witnessed by both mortal and spirit alike."
                                    );
                                    q.SendGump(new DialogueGump(q, ritualDetailModule));
                                });
                            ritualModule.AddOption("Describe the symbolism of your ritual elements.",
                                q => true,
                                q =>
                                {
                                    DialogueModule symbolModule = new DialogueModule(
                                        "The runes I inscribe are not mere symbols; they are portals to the divine. " +
                                        "The bonfire represents the eternal flame of life, and the incense carries prayers on its fragrant smoke. " +
                                        "Each element is a testament to the sacred balance between suffering and redemption. " +
                                        "It is through these rituals that I channel the will of the Shrouded Moon."
                                    );
                                    q.SendGump(new DialogueGump(q, symbolModule));
                                });
                            p.SendGump(new DialogueGump(p, ritualModule));
                        });

                    // Branch: The Sacred Rites of the Shrouded Moon
                    healingModule.AddOption("Reveal the sacred rites of the Shrouded Moon.",
                        p => true,
                        p =>
                        {
                            DialogueModule secretModule = new DialogueModule(
                                "This is not spoken of lightly... The Shrouded Moon is a deity shrouded in mystery and controversy. " +
                                "I was chosen by this divine force in a moment of blazing revelation—a vision that changed my fate forever. " +
                                "To those who are ready, I offer a glimpse into the secret rites that bind me to this sacred cause. " +
                                "Are you prepared to risk the unveiling of forbidden knowledge?"
                            );
                            secretModule.AddOption("Yes, I wish to know the truth.",
                                q => true,
                                q =>
                                {
                                    DialogueModule truthModule = new DialogueModule(
                                        "In a hidden sanctum beneath ancient ruins, I received a vision: a luminous figure robed in starlight, " +
                                        "whose eyes burned with the fervor of a thousand suns. The deity whispered, 'Embrace your destiny and let the divine " +
                                        "fire purge the ignorance of mortals.' Since that day, I have spread this message, urging the faithful to renounce the old " +
                                        "ways and embrace the new light of the Shrouded Moon. Will you join this sacred crusade?"
                                    );
                                    truthModule.AddOption("I am moved by your passion. Tell me more.",
                                        r => true,
                                        r =>
                                        {
                                            DialogueModule joinModule = new DialogueModule(
                                                "Then heed my call, seeker of truth! The path is arduous and fraught with peril, but the reward is enlightenment. " +
                                                "Our rituals are performed in secret, under moonlit skies, where we cleanse our souls with fervor and devotion. " +
                                                "Each act of piety, each sacrificial offering, brings us closer to the divine. " +
                                                "Are you willing to let your heart be transformed?"
                                            );
                                            joinModule.AddOption("I wish to walk this path.", 
                                                s => true,
                                                s =>
                                                {
                                                    DialogueModule commitmentModule = new DialogueModule(
                                                        "Your willingness is the first spark of a greater flame. " +
                                                        "Take this token as a symbol of your commitment, and remember that our sacred work continues in the shadows, " +
                                                        "bringing light to a darkened world."
                                                    );
                                                    s.SendGump(new DialogueGump(s, commitmentModule));
                                                });
                                            joinModule.AddOption("I need time to consider.", 
                                                s => true,
                                                s =>
                                                {
                                                    s.SendGump(new DialogueGump(s, CreateGreetingModule()));
                                                });
                                            r.SendGump(new DialogueGump(r, joinModule));
                                        });
                                    truthModule.AddOption("No, such secrets are too dangerous.",
                                        r => true,
                                        r =>
                                        {
                                            r.SendGump(new DialogueGump(r, CreateGreetingModule()));
                                        });
                                    q.SendGump(new DialogueGump(q, truthModule));
                                });
                            secretModule.AddOption("I fear the consequences of forbidden knowledge.",
                                q => true,
                                q =>
                                {
                                    DialogueModule cautionModule = new DialogueModule(
                                        "It is wise to be cautious—yet true enlightenment often demands sacrifice. " +
                                        "Remember, even the most radiant flame begins as a spark. " +
                                        "Only by embracing the full measure of truth can one overcome the shadows. " +
                                        "Should you ever change your mind, the path remains open."
                                    );
                                    q.SendGump(new DialogueGump(q, cautionModule));
                                });
                            p.SendGump(new DialogueGump(p, secretModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, healingModule));
                });

            // Option 2: Magical Research with Jonas (with extra nested details)
            greeting.AddOption("What insights have you and Jonas uncovered in your magical research?", 
                player => true,
                player =>
                {
                    DialogueModule magicModule = new DialogueModule(
                        "Our research into the arcane merges the tangible with the spiritual. " +
                        "Jonas is a brilliant scholar and together we study how elemental energies interact with divine forces. " +
                        "We pore over ancient manuscripts, decipher cryptic runes, and conduct experiments by the light of starlit nights. " +
                        "Would you care to delve into the theoretical foundations, the recent breakthroughs, or the philosophical implications of our work?"
                    );
                    
                    magicModule.AddOption("Describe the theoretical foundations.",
                        p => true,
                        p =>
                        {
                            DialogueModule theoryModule = new DialogueModule(
                                "We posit that every living soul resonates with a unique magical frequency—one that can be harnessed " +
                                "and amplified by the energies bestowed by the divine. Jonas discovered a long-forgotten treatise that " +
                                "suggests the interplay between mortal energy and celestial force is the key to unlocking advanced healing. " +
                                "Would you like to know about a particular experiment or the historical texts behind these ideas?"
                            );
                            theoryModule.AddOption("Tell me of an experiment.",
                                q => true,
                                q =>
                                {
                                    DialogueModule experimentModule = new DialogueModule(
                                        "In a recent experiment, Jonas and I combined a rare crystal, bathed in moonlight, with a tincture of Moonwort. " +
                                        "The crystal amplified the remedy’s potency, accelerating the healing process for severe wounds. " +
                                        "This fusion of science and divinity hints at a future where our understanding of magic might rival that of the gods themselves."
                                    );
                                    q.SendGump(new DialogueGump(q, experimentModule));
                                });
                            theoryModule.AddOption("What ancient texts support your findings?",
                                q => true,
                                q =>
                                {
                                    DialogueModule textsModule = new DialogueModule(
                                        "Among the texts we uncovered is a manuscript known as the 'Celestial Codex'—a work that predates even the oldest legends in Renika. " +
                                        "Its cryptic verses speak of divine energies interlacing with mortal efforts. A careful study reveals that the divine light of the Shrouded Moon " +
                                        "was once celebrated as the source of all healing powers. Intriguing, isn’t it?"
                                    );
                                    q.SendGump(new DialogueGump(q, textsModule));
                                });
                            p.SendGump(new DialogueGump(p, theoryModule));
                        });
                    
                    magicModule.AddOption("What practical breakthroughs have you achieved?",
                        p => true,
                        p =>
                        {
                            DialogueModule applicationModule = new DialogueModule(
                                "Our most promising breakthrough is the 'Harmony Salve'—a remedy whose healing power is magnified by subtle enchantments. " +
                                "This salve not only mends wounds but soothes the spirit, a synthesis of natural remedies and mystical energy. " +
                                "Would you like the detailed process of its creation, or the philosophical rationale behind merging magic with medicine?"
                            );
                            applicationModule.AddOption("Detail the creation process.",
                                q => true,
                                q =>
                                {
                                    DialogueModule creationModule = new DialogueModule(
                                        "The process begins with the precise grinding of enchanted herbs, followed by infusing the paste with a rare crystal " +
                                        "charged under the starlit sky. This meticulous process ensures that each application delivers not only medicinal benefits " +
                                        "but also a touch of divine grace."
                                    );
                                    q.SendGump(new DialogueGump(q, creationModule));
                                });
                            applicationModule.AddOption("Explain the philosophical rationale.",
                                q => true,
                                q =>
                                {
                                    DialogueModule philosophyModule = new DialogueModule(
                                        "We hold that healing is not merely a physical act, but a spiritual communion. " +
                                        "By merging the tangible and the metaphysical, we prove that the arcane is as much a part of our nature as the breath in our lungs. " +
                                        "This philosophy bridges the gap between mortal limitations and divine potential."
                                    );
                                    q.SendGump(new DialogueGump(q, philosophyModule));
                                });
                            p.SendGump(new DialogueGump(p, applicationModule));
                        });
                    
                    magicModule.AddOption("Contemplate the philosophical implications.",
                        p => true,
                        p =>
                        {
                            DialogueModule deepModule = new DialogueModule(
                                "The study of magic leads us to question the very fabric of existence. " +
                                "Is our mortal spark simply a fleeting ember, or could it be the start of a vast, divine conflagration? " +
                                "Our work challenges the boundaries between man and deity. Would you like to discuss the nature of fate, free will, or the destiny that binds us all?"
                            );
                            deepModule.AddOption("Discuss the nature of fate.",
                                q => true,
                                q =>
                                {
                                    DialogueModule fateModule = new DialogueModule(
                                        "Fate is like a river—ever-changing, yet carved by unseen forces. " +
                                        "In our research, we see glimpses of a predetermined path forged by divine will, " +
                                        "where every healing touch, every act of magic, steers us closer to an inevitable destiny."
                                    );
                                    q.SendGump(new DialogueGump(q, fateModule));
                                });
                            deepModule.AddOption("What about free will?",
                                q => true,
                                q =>
                                {
                                    DialogueModule freeWillModule = new DialogueModule(
                                        "Free will is the paradox of our existence. " +
                                        "While destiny may beckon, we choose our steps along the path—each decision a defiance of fate. " +
                                        "In our labors, both mortal and divine, free will is the spark that lights the eternal flame."
                                    );
                                    q.SendGump(new DialogueGump(q, freeWillModule));
                                });
                            deepModule.AddOption("Tell me how destiny influences your path.",
                                q => true,
                                q =>
                                {
                                    DialogueModule destinyModule = new DialogueModule(
                                        "My destiny was revealed in a vision—a moment when the shadows parted to reveal the light of the Shrouded Moon. " +
                                        "That divine spark now guides every step I take, fusing my healing art with a higher calling."
                                    );
                                    q.SendGump(new DialogueGump(q, destinyModule));
                                });
                            p.SendGump(new DialogueGump(p, deepModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, magicModule));
                });

            // Option 3: Community Restoration Projects with Torren (with extra detail)
            greeting.AddOption("Describe the community restoration projects you undertake with Torren.", 
                player => true,
                player =>
                {
                    DialogueModule restorationModule = new DialogueModule(
                        "Torren and I labor to restore our ravaged community spaces into sanctuaries of hope and renewal. " +
                        "Our projects are not merely physical repairs—they are spiritual rejuvenations. " +
                        "Together we rebuild ancient temples, revive forgotten gardens, and mend the broken streets, " +
                        "all while imbuing these spaces with the light of a higher purpose. Would you like to know about our current projects, " +
                        "the symbolic meaning behind them, or the secret ceremonies we perform during our restoration rituals?"
                    );
                    restorationModule.AddOption("What are your current projects?",
                        p => true,
                        p =>
                        {
                            DialogueModule currentModule = new DialogueModule(
                                "At present, we are renewing an ancestral healing garden near the central plaza—a place where nature and history entwine. " +
                                "Under Torren’s steady guidance, each stone and blossom is restored to honor the legacy of our forebears, " +
                                "and through secret ceremonies at night, we call upon divine forces to bless the land."
                            );
                            p.SendGump(new DialogueGump(p, currentModule));
                        });
                    restorationModule.AddOption("Explain the symbolic meaning behind these projects.",
                        p => true,
                        p =>
                        {
                            DialogueModule symbolModule = new DialogueModule(
                                "Every rebuilt structure, every replanted seed, echoes the rebirth of the community's spirit. " +
                                "It is a testament that even in decay, there is hope—an opportunity for resurrection. " +
                                "Each restoration is a prayer to the gods and a pledge to reclaim our sacred heritage."
                            );
                            p.SendGump(new DialogueGump(p, symbolModule));
                        });
                    restorationModule.AddOption("What are these secret ceremonies you mentioned?",
                        p => true,
                        p =>
                        {
                            DialogueModule ceremonyModule = new DialogueModule(
                                "At midnight, when the veil between worlds is at its thinnest, Torren and I gather in a long-forgotten shrine. " +
                                "There, we invoke ancient chants and light ceremonial torches—each flame representing a lost hope rekindled. " +
                                "The ritual is both a restoration of the physical space and a spiritual unburdening of our collective past."
                            );
                            ceremonyModule.AddOption("I would like to hear the chant.",
                                q => true,
                                q =>
                                {
                                    DialogueModule chantModule = new DialogueModule(
                                        "Listen closely: 'O radiant night, in thy shadow we trust, restore our hearts, renew our dust.' " +
                                        "These words, though simple, carry the weight of centuries and the promise of new beginnings."
                                    );
                                    q.SendGump(new DialogueGump(q, chantModule));
                                });
                            ceremonyModule.AddOption("What does this ritual achieve?",
                                q => true,
                                q =>
                                {
                                    DialogueModule achieveModule = new DialogueModule(
                                        "This ceremony draws the blessing of forgotten deities upon our lands, clearing away sorrow and filling hearts with renewed determination. " +
                                        "It is a quiet defiance against decay and a whispered promise of rebirth."
                                    );
                                    q.SendGump(new DialogueGump(q, achieveModule));
                                });
                            p.SendGump(new DialogueGump(p, ceremonyModule));
                        });
                    player.SendGump(new DialogueGump(player, restorationModule));
                });

            // Option 4: Seeking Illness Advice (detailed and nested)
            greeting.AddOption("I require guidance for an emerging illness.", 
                player => true,
                player =>
                {
                    DialogueModule illnessModule = new DialogueModule(
                        "Emerging illnesses are ominous omens that call for urgent intervention. " +
                        "In my work with Kess at Dawn, I've learned that symptoms such as unyielding fever, deep fatigue, and unsettling dreams " +
                        "might be more than mere ailments—they could signal a spiritual imbalance. " +
                        "Can you describe the symptoms in detail?"
                    );
                    illnessModule.AddOption("Fever, fatigue, and restless dreams plague me.",
                        p => true,
                        p =>
                        {
                            DialogueModule symptomModule = new DialogueModule(
                                "Such symptoms often denote an affliction that transcends simple infection. " +
                                "I would begin with a mild tonic—crafted from Moonwort and Silverleaf—followed by a ritual to realign the body's energy. " +
                                "Have you attempted any remedies yet?"
                            );
                            symptomModule.AddOption("No, what do you suggest?",
                                q => true,
                                q =>
                                {
                                    DialogueModule remedyModule = new DialogueModule(
                                        "I suggest you first sip a tonic prepared at dawn, allowing the herbal magic to quiet your fevered mind. " +
                                        "Then, under the fading light of day, attempt a gentle meditative ritual to restore balance to your spirit. " +
                                        "Should these fail, seek me again—for the divine often works in mysterious ways."
                                    );
                                    q.SendGump(new DialogueGump(q, remedyModule));
                                });
                            symptomModule.AddOption("Yes, but the remedies have not helped.",
                                q => true,
                                q =>
                                {
                                    DialogueModule advancedModule = new DialogueModule(
                                        "In dire cases such as these, a more potent remedy, imbued with the sacred fire of the Shrouded Moon, may be required. " +
                                        "I can prepare an elixir tailored specifically to your condition if you are willing to undertake a small quest to gather rare ingredients. " +
                                        "Would you be interested in such a venture?"
                                    );
                                    advancedModule.AddOption("Yes, I will gather the ingredients.",
                                        r => true,
                                        r =>
                                        {
                                            DialogueModule questModule = new DialogueModule(
                                                "Excellent. Seek out the blessed Nightbloom growing only in the shadowed glen, and retrieve a vial of pure spring water " +
                                                "from the hidden fountain deep within the ruins of Old Renika. Return to me, and we shall craft an elixir to vanquish your ailment."
                                            );
                                            r.SendGump(new DialogueGump(r, questModule));
                                        });
                                    advancedModule.AddOption("No, I'll wait and see.",
                                        r => true,
                                        r =>
                                        {
                                            r.SendGump(new DialogueGump(r, CreateGreetingModule()));
                                        });
                                    q.SendGump(new DialogueGump(q, advancedModule));
                                });
                            p.SendGump(new DialogueGump(p, symptomModule));
                        });
                    illnessModule.AddOption("I worry about a friend with these symptoms.",
                        p => true,
                        p =>
                        {
                            DialogueModule friendModule = new DialogueModule(
                                "Your compassion is a virtue indeed. " +
                                "I will immediately send a discreet message to Kess in Dawn so that additional remedies and healing aid are dispatched for your friend. " +
                                "In times of crisis, collective care is our greatest strength."
                            );
                            p.SendGump(new DialogueGump(p, friendModule));
                        });
                    player.SendGump(new DialogueGump(player, illnessModule));
                });

            // Option 5: Personal Journey and Secret Faith
            greeting.AddOption("Reveal more about your personal journey and faith.", 
                player => true,
                player =>
                {
                    DialogueModule journeyModule = new DialogueModule(
                        "My path has been forged in both light and shadow. " +
                        "Born amidst the ancient ruins of Renika, I was drawn to healing but soon discovered a higher calling—a fervent, divine mission bestowed upon me by a controversial deity, the Shrouded Moon. " +
                        "This revelation transformed me into not just a healer but a zealous priest, committed to a cause many fear and few understand. " +
                        "What facet of my journey intrigues you: my humble beginnings, my secret faith, or the transformative encounters with my companions Kess, Jonas, and Torren?"
                    );
                    journeyModule.AddOption("Tell me about your humble beginnings.",
                        p => true,
                        p =>
                        {
                            DialogueModule humbleModule = new DialogueModule(
                                "I grew up in the shadow of crumbling temples and lush groves, where the whispers of the past mingled with the hope of tomorrow. " +
                                "In those days, I witnessed both suffering and beauty—experiences that compelled me to alleviate pain and spread compassion. " +
                                "It was there I first felt the tug of something greater than mere mortal duty."
                            );
                            p.SendGump(new DialogueGump(p, humbleModule));
                        });
                    journeyModule.AddOption("Reveal your secret faith and divine calling.",
                        p => true,
                        p =>
                        {
                            DialogueModule faithModule = new DialogueModule(
                                "Under the silent gaze of the moon, I experienced a vision—a tumult of light and shadow that shattered all I once believed. " +
                                "In that sacred moment, the Shrouded Moon chose me, endowing me with the zeal to lead others toward a truth hidden from the uninitiated. " +
                                "Many label this faith as heretical, yet I believe it is the purest path to enlightenment. " +
                                "Are you ready to explore the secrets of this divine revelation?"
                            );
                            faithModule.AddOption("Yes, I wish to learn about this divine revelation.",
                                q => true,
                                q =>
                                {
                                    DialogueModule divineModule = new DialogueModule(
                                        "Very well. The Shrouded Moon speaks through the silence of night and the pulse of mortal hearts. " +
                                        "I was bathed in sacred light as I knelt before a crumbling altar beneath starry skies. " +
                                        "In that divine moment, I heard the voice: 'Awaken, devotee, and spread my light among the lost.' " +
                                        "Since then, I have dedicated my soul to awakening others. " +
                                        "Would you like to hear more about the sacred rites that bind me, or do you have questions about its implications for those who follow this path?"
                                    );
                                    divineModule.AddOption("Tell me about the sacred rites.",
                                        r => true,
                                        r =>
                                        {
                                            DialogueModule ritesModule = new DialogueModule(
                                                "The rites are shrouded in secrecy. By day, I heal and restore, but at night, in hidden groves, I gather with a select few. " +
                                                "We perform ancient chants, invoke the power of the lunar cycle, and offer personal sacrifices as symbols of our unwavering faith. " +
                                                "It is a practice both beautiful and terrifying in its intensity."
                                            );
                                            r.SendGump(new DialogueGump(r, ritesModule));
                                        });
                                    divineModule.AddOption("What are the implications for my own path?",
                                        r => true,
                                        r =>
                                        {
                                            DialogueModule implicationModule = new DialogueModule(
                                                "Embracing this divine calling is not without sacrifice. " +
                                                "Those who feel the stirring of the Shrouded Moon in their hearts must be willing to cast aside conventional fears, " +
                                                "to defy societal norms, and to walk in the light of a truth known only to the brave. " +
                                                "Will you dare to listen to the call?"
                                            );
                                            r.SendGump(new DialogueGump(r, implicationModule));
                                        });
                                    q.SendGump(new DialogueGump(q, divineModule));
                                });
                            faithModule.AddOption("No, such secrets are too unsettling for me.",
                                q => true,
                                q =>
                                {
                                    q.SendGump(new DialogueGump(q, CreateGreetingModule()));
                                });
                            p.SendGump(new DialogueGump(p, faithModule));
                        });
                    journeyModule.AddOption("How have your companions influenced you?",
                        p => true,
                        p =>
                        {
                            DialogueModule companionsModule = new DialogueModule(
                                "Kess, with her pragmatic care, grounds my more unbridled zeal. Jonas’s curious mind pushes me to explore the arcane mysteries, " +
                                "while Torren’s dedication to restoring our communal spaces reminds me that even the deepest faith must manifest in tangible hope. " +
                                "Each has shaped my journey in profound ways. Which relationship would you like to hear about first?"
                            );
                            companionsModule.AddOption("Tell me about your bond with Kess.",
                                r => true,
                                r =>
                                {
                                    DialogueModule kessModule = new DialogueModule(
                                        "Kess is both my partner and my counterweight. " +
                                        "Her steady hands and clear vision during times of crisis temper the more fervent aspects of my calling. " +
                                        "In the midst of an outbreak, it was her wisdom that ensured our rituals were grounded and effective."
                                    );
                                    r.SendGump(new DialogueGump(r, kessModule));
                                });
                            companionsModule.AddOption("Share your experiences with Jonas.",
                                r => true,
                                r =>
                                {
                                    DialogueModule jonasModule = new DialogueModule(
                                        "Jonas’s intellect is a beacon amid the dark unknowns of magic. " +
                                        "Our late-night discussions and experiments have revealed secrets that many would dismiss as myth. " +
                                        "It is a collaboration born from mutual respect and the hunger for truth."
                                    );
                                    r.SendGump(new DialogueGump(r, jonasModule));
                                });
                            companionsModule.AddOption("How does Torren inspire your work?",
                                r => true,
                                r =>
                                {
                                    DialogueModule torrenModule = new DialogueModule(
                                        "Torren’s passion for restoration is as fierce as my own zeal. " +
                                        "He sees beauty in ruins and potential where others see decay. " +
                                        "Working with him, I have come to understand that healing our land is an extension of healing our souls."
                                    );
                                    r.SendGump(new DialogueGump(r, torrenModule));
                                });
                            p.SendGump(new DialogueGump(p, companionsModule));
                        });
                    player.SendGump(new DialogueGump(player, journeyModule));
                });

            // Option 6: Farewell and Final Persuasion
            greeting.AddOption("Farewell.", 
                player => true,
                player =>
                {
                    DialogueModule farewellModule = new DialogueModule(
                        "May the guiding light of the Shrouded Moon illuminate your path, traveler. " +
                        "Remember, even in darkness, there is a spark of divine truth waiting to be ignited. " +
                        "Should you ever feel the call of destiny, seek me out, and together we shall transcend the limits of mortal despair."
                    );
                    player.SendGump(new DialogueGump(player, farewellModule));
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
