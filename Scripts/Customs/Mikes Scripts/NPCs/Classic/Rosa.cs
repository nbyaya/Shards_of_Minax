using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Rosa")]
public class Rosa : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public Rosa() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Rosa";
        Body = 0x191; // Human female body
        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        // Stats
        SetStr(80);
        SetDex(80);
        SetInt(80);
        SetHits(80);

        // Appearance
        AddItem(new Robe() { Hue = 1150 });
        AddItem(new Cloak() { Hue = 1150 });
        AddItem(new Sandals() { Hue = 1150 });
        AddItem(new Bonnet() { Hue = 1150 });
        AddItem(new Crossbow() { Name = "Rosa's Crossbow" });

        lastRewardTime = DateTime.MinValue; // Initialize lastRewardTime to a past time
    }

	public Rosa(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("I am Rosa, the sarcastic one. What do you want, hero?");

        greeting.AddOption("Tell me about your time at Baron Castle.",
            player => true,
            player =>
            {
                DialogueModule castleModule = new DialogueModule("Ah, Baron Castle! Those were the days. I was a young girl, wide-eyed and full of dreams. I trained to become a white mage, hoping to heal those in need.");
                
                castleModule.AddOption("What inspired you to become a healer?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule inspirationModule = new DialogueModule("It all started when I watched the healers mend the wounded after battles. The way they restored hope to families, I wanted to be part of that. It was magical.");
                        inspirationModule.AddOption("Did you have a mentor?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule mentorModule = new DialogueModule("Yes! Master Elowen was a wise old mage with a heart as big as her spells. She taught me the importance of compassion along with the art of healing.");
                                mentorModule.AddOption("What was Master Elowen like?",
                                    plb => true,
                                    plb =>
                                    {
                                        DialogueModule elowenModule = new DialogueModule("She was stern but fair, with a soft smile that could melt even the coldest hearts. I remember her saying, 'Healing is not just about magic; it's about understanding the pain of others.'");
                                        plb.SendGump(new DialogueGump(plb, elowenModule));
                                    });
                                mentorModule.AddOption("What lessons did you learn?",
                                    plb => true,
                                    plb =>
                                    {
                                        DialogueModule lessonsModule = new DialogueModule("Master Elowen taught me to listen first. To truly heal, one must understand the wounds, not just the symptoms. It’s about the connection you build with your patient.");
                                        plb.SendGump(new DialogueGump(plb, lessonsModule));
                                    });
                                player.SendGump(new DialogueGump(player, mentorModule));
                            });
                        inspirationModule.AddOption("What were your studies like?",
                            plb => true,
                            plb =>
                            {
                                DialogueModule studiesModule = new DialogueModule("Studying was intense! I spent countless hours in the library, pouring over ancient texts about herbs, potions, and spells. It was exhausting but exhilarating!");
                                studiesModule.AddOption("What was the hardest spell you learned?",
                                    plc => true,
                                    plc =>
                                    {
                                        DialogueModule hardestSpellModule = new DialogueModule("The hardest was the Healing Light spell. It required immense focus and a pure heart. I almost gave up, but Master Elowen believed in me and pushed me to try again.");
                                        plc.SendGump(new DialogueGump(plc, hardestSpellModule));
                                    });
                                plb.SendGump(new DialogueGump(plb, studiesModule));
                            });
                        player.SendGump(new DialogueGump(player, inspirationModule));
                    });

                castleModule.AddOption("What was your daily routine like?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule routineModule = new DialogueModule("Every morning began with meditation to center my mind. Then, I'd attend lectures and practical lessons on herbology and potion-making. The evenings were filled with practice and group healing sessions.");
                        routineModule.AddOption("Did you enjoy practicing with others?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule practiceModule = new DialogueModule("Absolutely! We were like a family, supporting one another. We’d share tips and experiences, making the learning process much more enjoyable.");
                                pla.SendGump(new DialogueGump(pla, practiceModule));
                            });
                        pl.SendGump(new DialogueGump(pl, routineModule));
                    });

                castleModule.AddOption("Were there any memorable experiences?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule memoriesModule = new DialogueModule("Oh, many! I remember once when a local lord came seeking aid for his sick daughter. It was a tense moment, but we managed to save her, and her father was eternally grateful.");
                        memoriesModule.AddOption("What happened after that?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule gratefulLordModule = new DialogueModule("He gifted us a beautiful tapestry that still hangs in the castle today. It depicted the bond between healer and patient, a reminder of why we do what we do.");
                                pla.SendGump(new DialogueGump(pla, gratefulLordModule));
                            });
                        player.SendGump(new DialogueGump(player, memoriesModule));
                    });

                player.SendGump(new DialogueGump(player, castleModule));
            });

        greeting.AddOption("What do you think about valor?",
            player => true,
            player =>
            {
                DialogueModule valorModule = new DialogueModule("Valor? In this place? You must be joking.");
                valorModule.AddOption("You seem to have lost hope.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, valorModule));
            });

        greeting.AddOption("Do you have any rewards for me?",
            player => DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10),
            player =>
            {
                lastRewardTime = DateTime.UtcNow; // Update the timestamp
                player.AddToBackpack(new MaxxiaScroll()); // Example reward item
                player.SendMessage("If you ever come across any inspiring sights in your adventures, bring them to me. Maybe, just maybe, I'll find the will to paint again.");
            });

        greeting.AddOption("What do you know about the ruins?",
            player => true,
            player =>
            {
                DialogueModule ruinsModule = new DialogueModule("Legend speaks of a guardian spirit that watches over those ruins. But like I said, just legends.");
                ruinsModule.AddOption("What kind of guardian?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule guardianModule = new DialogueModule("If you ever decide to confront this guardian, be prepared. Legends often have roots in reality.");
                        pl.SendGump(new DialogueGump(pl, guardianModule));
                    });
                player.SendGump(new DialogueGump(player, ruinsModule));
            });

        return greeting;
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write((int)0); // version
        writer.Write(lastRewardTime);
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
        lastRewardTime = reader.ReadDateTime();
    }
}
