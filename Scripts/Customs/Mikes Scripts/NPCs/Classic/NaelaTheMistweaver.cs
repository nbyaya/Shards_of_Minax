using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class NaelaTheMistweaver : BaseCreature
{
    [Constructable]
    public NaelaTheMistweaver() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Naela the Mistweaver";
        Body = 0x191; // Human female body
        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SetStr(50);
        SetDex(80);
        SetInt(110);
        SetHits(70);

        AddItem(new Cloak() { Hue = 1181 });
        AddItem(new SilverRing() { Name = "Naela's Band" });

        SpeechHue = 0; // Default speech hue
    }

    public NaelaTheMistweaver(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Naela the Mistweaver, a water genasi of ancient lineage. How may I assist you on your journey?");

        greeting.AddOption("Tell me about your abilities.",
            player => true,
            player => 
            {
                DialogueModule abilitiesModule = new DialogueModule("I harness the power of water to mend wounds and bring clarity to the mind. My essence flows like the tides, ever-changing and adaptive. What would you like to know?");
                
                abilitiesModule.AddOption("How do you heal?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule healingModule = new DialogueModule("Healing is an art, blending natural waters with my own essence. I channel the energy of the tides, allowing it to flow through me and into those in need. It's a delicate balance.");
                        healingModule.AddOption("That sounds powerful.",
                            p => true,
                            p => { p.SendGump(new DialogueGump(p, CreateGreetingModule())); });
                        abilitiesModule.AddOption("Tell me more about your methods.",
                            p => true,
                            p => { p.SendGump(new DialogueGump(p, healingModule)); });
                    });

                abilitiesModule.AddOption("What other skills do you have?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule skillsModule = new DialogueModule("Apart from healing, I am adept at water manipulation. I can create barriers of mist to obscure vision or summon currents to guide travelers through treacherous waters.");
                        skillsModule.AddOption("Can you teach me those skills?",
                            p => true,
                            p => { p.SendGump(new DialogueGump(p, skillsModule)); });
                        abilitiesModule.AddOption("That's incredible!",
                            p => true,
                            p => { p.SendGump(new DialogueGump(p, skillsModule)); });
                    });
                
                player.SendGump(new DialogueGump(player, abilitiesModule));
            });

        greeting.AddOption("What do you seek?",
            player => true,
            player =>
            {
                DialogueModule seekModule = new DialogueModule("The secrets of water are deep and unfathomable. Do you seek enlightenment, traveler? I can share wisdom from my lineage.");
                
                seekModule.AddOption("Yes, I seek enlightenment.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule yesModule = new DialogueModule("Wisdom is a river, and one must be willing to wade through its depths to find answers. What do you wish to learn about?");
                        
                        yesModule.AddOption("Tell me about your ancestry.",
                            p => true,
                            p =>
                            {
                                DialogueModule ancestryModule = new DialogueModule("My bloodline is steeped in the ancient waters of the river spirits. My ancestors were guardians of the streams, ensuring the balance of life was maintained. It is my duty to continue this legacy.");
                                ancestryModule.AddOption("That is fascinating!",
                                    pla => true,
                                    pla => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                                yesModule.AddOption("What can you teach me?",
                                    pla => true,
                                    pla => { pl.SendGump(new DialogueGump(pl, ancestryModule)); });
                            });

                        yesModule.AddOption("What about water magic?",
                            p => true,
                            p =>
                            {
                                DialogueModule magicModule = new DialogueModule("Water magic is a symphony of control and freedom. I can create waves to protect or heal, and even summon rain to nourish the land. Would you like to learn the basics?");
                                magicModule.AddOption("Yes, I want to learn!",
                                    plb => true,
                                    plb => { pl.SendGump(new DialogueGump(pl, magicModule)); });
                                yesModule.AddOption("Maybe another time.",
                                    plb => true,
                                    plb => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                            });

                        player.SendGump(new DialogueGump(player, yesModule));
                    });

                seekModule.AddOption("No, not right now.",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                
                player.SendGump(new DialogueGump(player, seekModule));
            });

        greeting.AddOption("Do you need any help?",
            player => true,
            player =>
            {
                DialogueModule helpModule = new DialogueModule("I appreciate your kindness, traveler. I often seek rare herbs from the Misty Grove. If you could assist me in gathering them, it would greatly aid my craft.");
                
                helpModule.AddOption("I would be glad to help.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule questModule = new DialogueModule("Thank you! The herbs I need are the Dewdrop Fern and the Shimmering Bloom. They grow near the water’s edge, often obscured by mist. Be cautious of the creatures that lurk nearby.");
                        questModule.AddOption("I will gather them for you.",
                            p => true,
                            p => { p.SendGump(new DialogueGump(p, CreateGreetingModule())); });
                        helpModule.AddOption("That sounds dangerous. I will pass.",
                            pln => true,
                            pln => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                        player.SendGump(new DialogueGump(player, questModule));
                    });

                helpModule.AddOption("What do these herbs do?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule herbInfoModule = new DialogueModule("The Dewdrop Fern enhances healing potions, while the Shimmering Bloom is known to clear the mind and strengthen resolve. Both are essential for my healing arts.");
                        herbInfoModule.AddOption("I see. I will help gather them.",
                            p => true,
                            p => { p.SendGump(new DialogueGump(p, CreateGreetingModule())); });
                        helpModule.AddOption("Sounds interesting, but I’ll pass.",
                            plm => true,
                            plm => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                        player.SendGump(new DialogueGump(player, herbInfoModule));
                    });

                player.SendGump(new DialogueGump(player, helpModule));
            });

        greeting.AddOption("What is the Misty Grove?",
            player => true,
            player =>
            {
                DialogueModule groveModule = new DialogueModule("The Misty Grove is a sacred place, draped in fog and filled with mystical energies. It is said that those who enter with pure intentions may find guidance from the spirits of the waters.");
                
                groveModule.AddOption("That sounds enchanting!",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                groveModule.AddOption("Are there dangers there?",
                    pl => true,
                    pl => 
                    {
                        DialogueModule dangerModule = new DialogueModule("Yes, the grove is home to various creatures that may not take kindly to intruders. It is wise to be cautious and prepared before venturing into its depths.");
                        dangerModule.AddOption("I will prepare myself before visiting.",
                            p => true,
                            p => { p.SendGump(new DialogueGump(p, CreateGreetingModule())); });
                        groveModule.AddOption("Thanks for the warning!",
                            pls => true,
                            pls => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                        player.SendGump(new DialogueGump(player, dangerModule));
                    });

                player.SendGump(new DialogueGump(player, groveModule));
            });

        greeting.AddOption("Goodbye for now.",
            player => true,
            player =>
            {
                player.SendMessage("Naela waves gently, her form shimmering like the water's surface.");
            });

        return greeting;
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
