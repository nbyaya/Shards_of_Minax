using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Crag the Miner")]
    public class Crag : BaseCreature
    {
        [Constructable]
		public Crag() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Crag";
            Body = 0x190; // Standard human body

            // Stats reflecting a rugged miner with hidden depths
            SetStr(150);
            SetDex(60);
            SetInt(70);
            SetHits(120);

            // Appearance: practical miner garb with a hint of personal flair
            AddItem(new FancyShirt() { Hue = 2850 });
            AddItem(new ShortPants() { Hue = 1150 });
            AddItem(new Boots() { Hue = 1150 });
            AddItem(new Pickaxe() { Name = "Old Reliable" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
        }

        public Crag(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            // Crag introduces himself—on the surface a miner haunted by voices,
            // yet with a hidden passion once channeled through alchemy.
            DialogueModule greeting = new DialogueModule(
                "Greetings, traveler. I'm Crag, a miner of Devil Guard, where the old stones whisper secrets and the voices of the past " +
                "echo in these deep tunnels. Some believe my ears merely pick up the wind through ancient corridors—but I suspect there's more " +
                "to it. What would you like to learn about?");

            // Option 1: About the voices in the mines.
            greeting.AddOption("Tell me about the voices in the mines.",
                player => true,
                player =>
                {
                    DialogueModule voicesModule = new DialogueModule(
                        "The voices... They have been my unwelcome companions since I first set foot in these dark shafts. " +
                        "Sometimes, they warn me of unstable rock and hidden caverns; other times, they recite forgotten lore of ancient builders. " +
                        "Their tone can be eerie, sometimes soothing, as if sharing secrets meant only for those who truly listen. What aspect fascinates you?");
                    
                    voicesModule.AddOption("What messages do they bring?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule messagesModule = new DialogueModule(
                                "They whisper of impending collapse, caution me about crumbling support, and even hint at hidden ore veins " +
                                "rich with magic. There was one instance when I heard them warn of a 'spark from the deep'—I later discovered it was " +
                                "no mere accident, but an echo of my own past experiments.");
                            pl.SendGump(new DialogueGump(pl, messagesModule));
                        });
                    
                    voicesModule.AddOption("Do they guide your work?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule guideModule = new DialogueModule(
                                "Indeed. When the voices rise and fall in a peculiar rhythm, I know danger—or fortune—awaits. " +
                                "Their guidance has saved lives and, in more than one instance, nudged me toward secret discoveries. Sometimes, I wonder " +
                                "if they carry remnants of a magic I once sought to master.");
                            pl.SendGump(new DialogueGump(pl, guideModule));
                        });
                    
                    // Additional nested branch linking subtly to his secret past.
                    voicesModule.AddOption("Do the voices ever hint at... hidden magic?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule hiddenMagicModule = new DialogueModule(
                                "Occasionally, yes. There are moments when their whispers seem to recall the shimmer of alchemical reactions— " +
                                "the glow of potions once brewed in secret by a curious mind. But that's a tale best saved for another time.");
                            pl.SendGump(new DialogueGump(pl, hiddenMagicModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, voicesModule));
                });

            // Option 2: About Sir Thomund’s security advice.
            greeting.AddOption("What advice does Sir Thomund offer you?",
                player => true,
                player =>
                {
                    DialogueModule thomundModule = new DialogueModule(
                        "Sir Thomund of Castle British is more than a noble in shining armor—he's a vigilant guardian of these ancient halls. " +
                        "He often inspects our tunnels, advising on enchanted reinforcements and practical ways to secure the mine. " +
                        "His advice blends solid construction with hints of mystical protection, as if he too knows of deeper secrets in the stone.");
                    
                    thomundModule.AddOption("What specific measures does he recommend?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule measuresModule = new DialogueModule(
                                "He speaks of reinforcing supports with enchanted timbers, installing wards against restless spirits, and even " +
                                "using forgotten construction techniques that harness residual magic. It's as if every stone and rune has a story " +
                                "waiting to be guarded.");
                            pl.SendGump(new DialogueGump(pl, measuresModule));
                        });
                    
                    thomundModule.AddOption("I sense his concern is more than practical.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule caringModule = new DialogueModule(
                                "Absolutely. Sir Thomund believes that by preserving these ancient ways and protecting our work, " +
                                "we honor not just the past, but the unseen energies that pulse beneath our feet. His guidance is both pragmatic and reverent.");
                            pl.SendGump(new DialogueGump(pl, caringModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, thomundModule));
                });

            // Option 3: About mining lore exchanged with Yorn.
            greeting.AddOption("What mining lore have you shared with Yorn?",
                player => true,
                player =>
                {
                    DialogueModule loreModule = new DialogueModule(
                        "Yorn is the wise elder of our mining community—a living repository of lore and ancient tales. " +
                        "Together, we decipher cryptic carvings and listen to the silent testimonies of the rock. His wisdom tells " +
                        "stories of titanic hammers, secret veins, and even of magical experiments that once blurred the line between science and wonder.");
                    
                    loreModule.AddOption("What secrets does that lore reveal?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule secretsModule = new DialogueModule(
                                "The lore speaks of an era when magic and mining intertwined. It tells of rituals to awaken the power of minerals, " +
                                "and of alchemical experiments that transformed humble ore into substances of uncanny properties. Each story deepens the mystique " +
                                "of our craft.");
                            pl.SendGump(new DialogueGump(pl, secretsModule));
                        });
                    
                    loreModule.AddOption("How does Yorn help you understand the mine?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule yornModule = new DialogueModule(
                                "Yorn’s insight is like a lantern in the dark. He interprets the subtle shifts in the rock—how a fracture " +
                                "or a glimmer signals hidden pathways. His wisdom has even led me to recall memories of a time when I dabbled in " +
                                "more mysterious experiments.");
                            pl.SendGump(new DialogueGump(pl, yornModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, loreModule));
                });

            // Option 4: About working with Kel on ancient inscriptions.
            greeting.AddOption("I heard you work with Kel on ancient inscriptions. Tell me about that.",
                player => true,
                player =>
                {
                    DialogueModule kelModule = new DialogueModule(
                        "Kel is a scholar at heart—a keeper of forbidden texts and runes. When I stumble upon carvings deep in the tunnels, " +
                        "I contact him. Together, we try to decode these relics, piecing together lost languages and enigmatic formulas. " +
                        "Sometimes, these inscriptions hint at alchemical recipes and powers beyond the mundane.");
                    
                    kelModule.AddOption("What do these inscriptions reveal?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule inscriptionsDetailModule = new DialogueModule(
                                "Some tell of blessings from ancient earth-gods that promise prosperity if the proper rituals are observed. " +
                                "Others warn of curses for those who disturb what is sacred. I have often wondered if these runes were penned by an alchemist " +
                                "long ago—one who dared to mix magic and ore in ways we can hardly imagine.");
                            pl.SendGump(new DialogueGump(pl, inscriptionsDetailModule));
                        });
                    
                    kelModule.AddOption("How do you and Kel work together?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule collaborateModule = new DialogueModule(
                                "Our collaboration is a meeting of brawn and brains. I bring him raw findings and curious anomalies from the field, " +
                                "and he spends nights poring over ancient scrolls and deciphering cryptic symbols. Our debates often stretch long into the night, " +
                                "sparked by candlelight and the thrill of uncovering long-hidden secrets.");
                            pl.SendGump(new DialogueGump(pl, collaborateModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, kelModule));
                });

            // Option 5: NEW – Secret dialogue about his hidden past as an alchemist.
            greeting.AddOption("I hear whispers of experiments beyond mining... Were you ever an alchemist?",
                player => true,
                player =>
                {
                    DialogueModule alchemyModule = new DialogueModule(
                        "Crag lowers his voice, his rugged face softening with a secretive glimmer. 'You're sharper than most, friend. " +
                        "Not many know this, but there was a time when I was not only a miner but also a curious, eccentric tinkerer of alchemy. " +
                        "In the quiet hours beneath these rocky slopes, I dabbled in transmuting minerals and rare herbs into wondrous—and sometimes dangerous—potions.'");

                    alchemyModule.AddOption("What kind of experiments did you conduct?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule expModule = new DialogueModule(
                                "Ah, the experiments! I was driven by an insatiable curiosity and inventive flair. I mixed powdered ore with " +
                                "exotic botanicals, brewing potions that glowed with an otherworldly radiance. One of my proudest yet risky creations " +
                                "was a luminous elixir—born from silver ore dust and dew gathered under a full moon. It was meant to reveal hidden ore veins, " +
                                "but occasionally, it sparked an unexpected reaction.");
                            
                            expModule.AddOption("Tell me more about the luminous elixir.",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule glowPotionModule = new DialogueModule(
                                        "That potion was a marvel and a menace. With a sparkle reminiscent of starlight, it briefly lit up the dark caverns " +
                                        "with its ethereal glow. I intended it as a tool to locate the rarest minerals, yet it also produced small, unpredictable bursts " +
                                        "of energy that nearly singed my beard off. A perfect blend of beauty and calamity, if I may say so.");
                                    pl2.SendGump(new DialogueGump(pl2, glowPotionModule));
                                });
                            
                            expModule.AddOption("Did any experiment have disastrous consequences?",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule disasterModule = new DialogueModule(
                                        "Oh, the mishaps were many. I once endeavored to bind the volatile essence of molten rock into a stable compound, " +
                                        "only to trigger a reaction that nearly sealed off an entire tunnel. The explosion reverberated through the mine, " +
                                        "leaving scars on both the rock and my conscience. It was a stark reminder that even the most brilliant ideas can have catastrophic sides.");
                                    pl2.SendGump(new DialogueGump(pl2, disasterModule));
                                });
                            
                            pl.SendGump(new DialogueGump(pl, expModule));
                        });

                    alchemyModule.AddOption("Where did you practice your secret experiments?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule labModule = new DialogueModule(
                                "I fashioned a hidden alcove deep within the labyrinth of Devil Guard, concealed behind an old, forgotten shaft. " +
                                "It was a cramped, dim chamber with quartz-lined walls and makeshift apparatus cobbled together from spare mining tools. " +
                                "There, under the flicker of unstable alchemical flames, I let my inventive spirit roam free away from prying eyes.");
                            
                            labModule.AddOption("What did your secret lab look like?",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule labVisualModule = new DialogueModule(
                                        "Picture a small chamber where shadows dance on walls streaked with mineral veins. " +
                                        "Bubbling brews in cracked copper vessels, erratic puffs of colored smoke, and scattered instruments of both " +
                                        "mining and mysticism created an atmosphere of chaotic genius—a sanctum where science and magic met in a delicate balance.");
                                    pl2.SendGump(new DialogueGump(pl2, labVisualModule));
                                });
                            
                            labModule.AddOption("Do you ever return to your lab?",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule labReturnModule = new DialogueModule(
                                        "Sometimes, on nights when the voices echo louder than usual, I feel an irresistible pull back to that secret chamber. " +
                                        "But the risks of rekindling old magic keep me grounded in my mining work. The lab remains a bittersweet memory of what once was " +
                                        "and a reminder of the dangers inherent in unbridled invention.");
                                    pl2.SendGump(new DialogueGump(pl2, labReturnModule));
                                });
                            
                            pl.SendGump(new DialogueGump(pl, labModule));
                        });
                    
                    alchemyModule.AddOption("Why did you abandon alchemy to become a full-time miner?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule whyModule = new DialogueModule(
                                "It wasn’t a simple decision, I assure you. My experiments—though imbued with wonder and unorthodox brilliance—often " +
                                "tended toward the catastrophic. I realized that the risks weren’t just mine to bear; they threatened the safety of everyone " +
                                "around me. I chose the steady, honest labor of mining, where every rock tells its tale, over the volatile allure of hidden magic.");
                            
                            whyModule.AddOption("Do you ever regret leaving alchemy behind?",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule regretModule = new DialogueModule(
                                        "Regret? Perhaps a tinge, in quiet moments when the echoes of past experiments linger like ghostly refractions. " +
                                        "Yet, every misstep and near-disaster taught me the price of recklessness. My life here, as hard as the stone, " +
                                        "remains a testament to finding balance between brilliance and safety.");
                                    pl2.SendGump(new DialogueGump(pl2, regretModule));
                                });
                            
                            whyModule.AddOption("What do you miss most about your alchemical days?",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule missModule = new DialogueModule(
                                        "I miss the thrill of discovery, the spark of inspiration when a new formula revealed an unexpected wonder. " +
                                        "There was a time when every experiment was an adventure, an exploration into the unknown realms of magic and matter. " +
                                        "But the price of that pursuit grew too steep to bear for those around me.");
                                    pl2.SendGump(new DialogueGump(pl2, missModule));
                                });
                            
                            pl.SendGump(new DialogueGump(pl, whyModule));
                        });

                    alchemyModule.AddOption("Have you resumed any experiments lately?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule resumeModule = new DialogueModule(
                                "In isolated moments, I toy with small experiments—small enough not to upset the delicate balance of the mine. " +
                                "Recently, I attempted to fuse a rare cobalt ore with a tincture from a luminous fungus. The reaction was brief " +
                                "but left the cavern bathed in an eerie blue glow. It reminded me that the spark of invention still flickers within me.");
                            
                            resumeModule.AddOption("What happened during that experiment?",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule recentModule = new DialogueModule(
                                        "The mixture was as volatile as it was beautiful—a precise spark ignited an ephemeral glow that faded " +
                                        "just as quickly as it emerged, leaving behind only the memory of its brilliance. It was a calculated risk, " +
                                        "a dance with chaos that ultimately proved both captivating and humbling.");
                                    pl2.SendGump(new DialogueGump(pl2, recentModule));
                                });
                            
                            resumeModule.AddOption("Will you try more alchemical experiments?",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule futureModule = new DialogueModule(
                                        "Perhaps, if the stars align and the mine’s murmurs permit. My inventive nature still yearns for the thrill of discovery— " +
                                        "but wisdom now tempers my ambition. Each step forward is measured, every experiment a delicate balance between wonder and risk.");
                                    pl2.SendGump(new DialogueGump(pl2, futureModule));
                                });
                            
                            pl.SendGump(new DialogueGump(pl, resumeModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, alchemyModule));
                });

            // Option 6: End the conversation.
            greeting.AddOption("I have no time for mining tales, thank you.",
                player => true,
                player =>
                {
                    DialogueModule farewellModule = new DialogueModule(
                        "Very well, friend. May the earth grant you safe passage, and may the silent voices always guide your steps.");
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
