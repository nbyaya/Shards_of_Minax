using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Shadow")]
public class Shadow : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public Shadow() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Shadow";
        Body = 0x190; // Human male body

        // Stats
        Str = 120;
        Dex = 120;
        Int = 60;
        Hits = 85;

        // Appearance
        AddItem(new LeatherLegs() { Hue = 1100 });
        AddItem(new LeatherCap() { Hue = 1100 });
        AddItem(new LeatherGloves() { Hue = 1100 });
        AddItem(new ChainChest() { Hue = 1100 });
        AddItem(new Kryss() { Name = "Shadow's Blade" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
        
        SpeechHue = 0; // Default speech hue

        lastRewardTime = DateTime.MinValue;
    }

    public Shadow(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Hmph, what do you want?");

        greeting.AddOption("Tell me your story.",
            player => true,
            player =>
            {
                DialogueModule storyModule = new DialogueModule("Abandoned to die on a magical floating continent, I learned the true meaning of survival. It wasn't just a place; it was a prison of my own making.");
                storyModule.AddOption("What happened on that continent?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule continentModule = new DialogueModule("I was betrayed by those I trusted, left behind as they fled to save themselves. The continent was beautiful, but it hid dark secrets. I had to escape before it exploded.");
                        continentModule.AddOption("What caused the explosion?",
                            p => true,
                            p =>
                            {
                                DialogueModule explosionModule = new DialogueModule("The very magic that kept it afloat began to unravel. Ancient rituals gone awry. I could feel the ground tremble beneath me as chaos erupted.");
                                explosionModule.AddOption("How did you manage to escape?",
                                    plq => true,
                                    plq =>
                                    {
                                        DialogueModule escapeModule = new DialogueModule("I found an ancient relic—a portal stone. It flickered with energy, a last chance for salvation. With no time to spare, I activated it, leaping into the unknown.");
                                        escapeModule.AddOption("What happened next?",
                                            pla => true,
                                            pla =>
                                            {
                                                DialogueModule afterEscapeModule = new DialogueModule("I landed in a place I did not recognize, alone and battered. The journey had changed me. I was no longer just Shadow; I was a survivor.");
                                                afterEscapeModule.AddOption("Do you still feel the scars of that betrayal?",
                                                    pw => true,
                                                    pw =>
                                                    {
                                                        p.SendGump(new DialogueGump(p, new DialogueModule("Every day. The weight of trust is heavy. I tread lightly around others, fearing betrayal again.")));
                                                    });
                                                pla.SendGump(new DialogueGump(pla, afterEscapeModule));
                                            });
                                    });
                                p.SendGump(new DialogueGump(p, explosionModule));
                            });
                        pl.SendGump(new DialogueGump(pl, continentModule));
                    });

                storyModule.AddOption("What did you learn from that experience?",
                    playere => true,
                    playere =>
                    {
                        DialogueModule lessonModule = new DialogueModule("Survival isn't just about the body; it's about the mind. Trust must be earned, not freely given. And sometimes, the only ally you have is yourself.");
                        lessonModule.AddOption("Do you ever regret surviving?",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Regret is a poison. I may have survived, but the memories haunt me. What would you do if you were in my place?")));
                            });
                        player.SendGump(new DialogueGump(player, lessonModule));
                    });

                greeting.AddOption("What did you find on that continent?",
                    playerr => true,
                    playerr =>
                    {
                        DialogueModule findModule = new DialogueModule("Ancient relics, forgotten magic, and the whispers of the lost. I found knowledge that could change destinies, but at what cost?");
                        findModule.AddOption("What knowledge?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule knowledgeModule = new DialogueModule("Secrets of the arcane, tomes filled with power. But they also held the weight of responsibility. Knowledge can be both a gift and a curse.");
                                knowledgeModule.AddOption("Did you use any of it?",
                                    p => true,
                                    p =>
                                    {
                                        DialogueModule usedModule = new DialogueModule("I've used some, but I tread carefully. Magic that saved me could also destroy me if mishandled. You understand that, don’t you?");
                                        usedModule.AddOption("I do, but what if it could help others?",
                                            pla => true,
                                            pla =>
                                            {
                                                pla.SendGump(new DialogueGump(pla, new DialogueModule("Helping others is noble, but can lead to more pain. Balance is key; use it wisely.")));
                                            });
                                        p.SendGump(new DialogueGump(p, usedModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, knowledgeModule));
                            });
                        player.SendGump(new DialogueGump(player, findModule));
                    });

                player.SendGump(new DialogueGump(player, storyModule));
            });

        greeting.AddOption("What about the Crimson Dagger?",
            player => true,
            player =>
            {
                DialogueModule daggerModule = new DialogueModule("Ah, the Crimson Dagger, a weapon of legends. It holds power, but finding it is perilous. Are you brave enough to seek it?");
                daggerModule.AddOption("What will you give me if I find it?",
                    pl => true,
                    pl =>
                    {
                        TimeSpan cooldown = TimeSpan.FromMinutes(10);
                        if (DateTime.UtcNow - lastRewardTime < cooldown)
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("I have no reward right now. Please return later.")));
                        }
                        else
                        {
                            pl.AddToBackpack(new PlateLeggingsOfCommand()); // Give the reward
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Take this for your efforts.")));
                        }
                    });
                player.SendGump(new DialogueGump(player, daggerModule));
            });

        greeting.AddOption("Tell me about your scars.",
            player => true,
            player =>
            {
                DialogueModule scarsModule = new DialogueModule("Every scar has a tale. Some of victory, others of loss. Each mark tells a story of survival, of battles fought.");
                scarsModule.AddOption("What scars do you carry?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Scars of betrayal, wounds of survival. They remind me that I'm alive, and that I must remain vigilant.")));
                    });
                player.SendGump(new DialogueGump(player, scarsModule));
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
