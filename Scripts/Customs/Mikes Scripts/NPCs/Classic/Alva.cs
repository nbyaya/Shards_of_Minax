using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Alva")]
    public class Alva : BaseCreature
    {
        [Constructable]
        public Alva() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Alva";
            Body = 0x191; // Human female body
            Hue = 0x2002; // Clothing Hue

            // Stats
            SetStr(90);
            SetDex(100);
            SetInt(70);
            SetHits(75);

            // Appearance
            AddItem(new FancyShirt(0x2002)); // Fancy Dress with hue 2002
            AddItem(new Boots(0x2002)); // Boots with hue 2002
            AddItem(new GnarledStaff { Name = "Alva's Staff" });

            SpeechHue = 0; // Default speech hue
        }

        public Alva(Serial serial) : base(serial)
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
            DialogueModule greeting = new DialogueModule("I am Alva, the explorer of forgotten worlds. What brings you to my humble company?");
            
            greeting.AddOption("Tell me about your journeys.",
                player => true,
                player =>
                {
                    DialogueModule journeysModule = new DialogueModule("I've journeyed through the forgotten ruins of Sarn and the perilous depths of Vaal. My expeditions have uncovered untold stories and artifacts.");
                    journeysModule.AddOption("What treasures have you found?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule treasuresModule = new DialogueModule("Ah, treasures. They can be both a blessing and a curse. Many are the relics I've found, but the Orb of Lost Echoes is the most enigmatic of all.");
                            treasuresModule.AddOption("What is the Orb of Lost Echoes?",
                                p => true,
                                p =>
                                {
                                    DialogueModule orbModule = new DialogueModule("The Orb of Lost Echoes is said to contain the voices and memories of those long gone. Holding it, one can hear whispers from the past. But I've only glimpsed it in ancient texts.");
                                    orbModule.AddOption("Interesting! Do you have a map?",
                                        plq => true,
                                        plq =>
                                        {
                                            pl.SendGump(new DialogueGump(pl, CreateMapModule()));
                                        });
                                    p.SendGump(new DialogueGump(p, orbModule));
                                });
                            treasuresModule.AddOption("Sounds intriguing.",
                                plw => true,
                                plw =>
                                {
                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                });
                            player.SendGump(new DialogueGump(player, treasuresModule));
                        });
                    journeysModule.AddOption("Tell me about Sarn.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule sarnModule = new DialogueModule("Ah, Sarn, the ancient city filled with memories of a bygone era. I once uncovered a map there that hinted at a secret chamber deep below the city.");
                            sarnModule.AddOption("What secrets does the map hold?",
                                p => true,
                                p =>
                                {
                                    p.SendGump(new DialogueGump(p, CreateMapModule()));
                                });
                            player.SendGump(new DialogueGump(player, sarnModule));
                        });
                    player.SendGump(new DialogueGump(player, journeysModule));
                });

            greeting.AddOption("Why do you call yourself frail?",
                player => true,
                player =>
                {
                    DialogueModule frailModule = new DialogueModule("While my body may be weak, I've fortified my mind with knowledge and experiences from my journeys. Do you know about the Elixir of Youth I once sought?");
                    frailModule.AddOption("Tell me about the Elixir of Youth.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule elixirModule = new DialogueModule("The Elixir of Youth, a legendary potion said to grant eternal youth. I once believed it was real. While I never found the actual elixir, the journey taught me more about life than any potion ever could. For your curiosity, take this.");
                            elixirModule.AddOption("Thank you!",
                                p => true,
                                p =>
                                {
                                    p.AddToBackpack(new MaxxiaScroll()); // Assuming MaxxiaScroll is a valid item
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            player.SendGump(new DialogueGump(player, elixirModule));
                        });
                    player.SendGump(new DialogueGump(player, frailModule));
                });

            greeting.AddOption("Can you share a secret?",
                player => true,
                player =>
                {
                    DialogueModule secretModule = new DialogueModule("The map I uncovered was old, worn, and barely legible. It spoke of a chamber where time stood still. I've been meaning to investigate, but dangers lurk in the shadows of Sarn.");
                    secretModule.AddOption("I might join you in your investigation.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, secretModule));
                });

            greeting.AddOption("What can you tell me about time portals?",
                player => true,
                player =>
                {
                    DialogueModule portalsModule = new DialogueModule("Ah, time portals! A powerful and dangerous art. I can summon them for a short time by sacrificing my blood in a Vaal ritual. This ancient practice allows me to peer into the fabric of time.");
                    portalsModule.AddOption("How does the ritual work?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule ritualModule = new DialogueModule("The ritual requires rare ingredients and, of course, a small offering of my own blood. I must chant the ancient incantations while drawing a sigil of the Vaal with my blood.");
                            ritualModule.AddOption("What ingredients do you need?",
                                p => true,
                                p =>
                                {
                                    DialogueModule ingredientsModule = new DialogueModule("To perform the ritual, I need a shard of Vaal crystal, a drop of Essence of Time, and a fragment of temporal dust from the ruins of Sarn. Each item is dangerous to obtain, but crucial for the ritual.");
                                    ingredientsModule.AddOption("Where can I find these items?",
                                        ple => true,
                                        ple =>
                                        {
                                            DialogueModule locationsModule = new DialogueModule("The Vaal crystal can be found deep within the ruins of Sarn, guarded by ancient beings. The Essence of Time is rumored to be located at the Temporal Nexus, a hidden site that shifts through time itself. The temporal dust can be collected from the ruins, but beware of lurking dangers.");
                                            locationsModule.AddOption("What dangers should I expect?",
                                                prr => true,
                                                prr =>
                                                {
                                                    p.SendMessage("You must be wary of time-warped creatures and traps set by those who once protected these ancient places. Many have entered but never returned.");
                                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                                });
                                            locationsModule.AddOption("That sounds perilous. Is it worth it?",
                                                pt => true,
                                                pt =>
                                                {
                                                    DialogueModule worthItModule = new DialogueModule("The knowledge and power one can gain from harnessing the essence of time is invaluable. It could allow one to glimpse the past or even alter the future!");
                                                    worthItModule.AddOption("I am intrigued! How can I assist?",
                                                        ply => true,
                                                        ply =>
                                                        {
                                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                        });
                                                    p.SendGump(new DialogueGump(p, worthItModule));
                                                });
                                            pl.SendGump(new DialogueGump(pl, locationsModule));
                                        });
                                    ingredientsModule.AddOption("I can help gather these ingredients.",
                                        plu => true,
                                        plu =>
                                        {
                                            pl.SendMessage("Your assistance would be invaluable! But remember, once the ritual is complete, the portals remain open only for a brief moment. Timing is everything.");
                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                        });
                                    player.SendGump(new DialogueGump(player, ingredientsModule));
                                });
                            portalsModule.AddOption("What are the risks of summoning a portal?",
                                pli => true,
                                pli =>
                                {
                                    DialogueModule risksModule = new DialogueModule("Summoning a portal requires precision. If performed incorrectly, the portal could become unstable, leading to unpredictable consequences. One could be lost in time or attract unwanted entities.");
                                    risksModule.AddOption("I will be cautious.",
                                        p => true,
                                        p =>
                                        {
                                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                        });
                                    risksModule.AddOption("This seems too dangerous for me.",
                                        p => true,
                                        p =>
                                        {
                                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                        });
                                    player.SendGump(new DialogueGump(player, risksModule));
                                });
                            pl.SendGump(new DialogueGump(pl, ritualModule));
                        });
                    portalsModule.AddOption("What happens when you summon a portal?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule summonModule = new DialogueModule("When I summon a portal, it flickers into existence, a swirling vortex of time and space. For a fleeting moment, one can step through and witness events from the past, or even catch a glimpse of the future.");
                            summonModule.AddOption("Can I try it?",
                                p => true,
                                p =>
                                {
                                    p.SendMessage("It is not something to take lightly! Only those truly prepared should attempt to cross the threshold of time.");
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            summonModule.AddOption("What do you see when you cross?",
                                p => true,
                                p =>
                                {
                                    p.SendMessage("I have seen the rise and fall of empires, the faces of heroes long forgotten, and the choices that shaped our world. But beware; knowledge can be both a gift and a burden.");
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            player.SendGump(new DialogueGump(player, summonModule));
                        });
                    player.SendGump(new DialogueGump(player, portalsModule));
                });

            greeting.AddOption("Goodbye, Alva.",
                player => true,
                player =>
                {
                    player.SendMessage("Alva nods and bids you farewell.");
                });

            return greeting;
        }

        private DialogueModule CreateMapModule()
        {
            DialogueModule mapModule = new DialogueModule("The map was old, worn, and barely legible. It spoke of a chamber where time stood still. What do you wish to know about it?");
            mapModule.AddOption("Where can I find this chamber?",
                pl => true,
                pl =>
                {
                    pl.SendMessage("Alva points towards the distant ruins of Sarn, warning you of the dangers that await.");
                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                });
            mapModule.AddOption("What dangers lurk there?",
                pl => true,
                pl =>
                {
                    pl.SendMessage("Many dangers lurk in the shadows of Sarn—ancient guardians and traps protect its secrets.");
                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                });
            return mapModule;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
