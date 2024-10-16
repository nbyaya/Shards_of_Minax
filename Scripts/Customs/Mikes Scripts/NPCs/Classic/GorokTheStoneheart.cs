using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Gorok the Stoneheart")]
public class GorokTheStoneheart : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public GorokTheStoneheart() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Gorok the Stoneheart";
        Body = 0x190; // Human male body

        // Stats
        Str = 150;
        Dex = 40;
        Int = 50;
        Hits = 120;

        // Appearance
        AddItem(new PlateChest() { Hue = 1176 });
        AddItem(new PlateLegs() { Hue = 1176 });
        AddItem(new WarHammer() { Name = "Gorok's Maul" });

        Hue = 1175;
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public GorokTheStoneheart(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("I am Gorok the Stoneheart, a miserable creature of earth and stone, bound to this place for eternity.");

        greeting.AddOption("Tell me about your existence.",
            player => true,
            player =>
            {
                DialogueModule existenceModule = new DialogueModule("My existence spans eons, a long journey of solitude and reflection. I have witnessed the rise and fall of empires, the ebb and flow of life itself.");
                existenceModule.AddOption("What do you mean by solitude?",
                    p => true,
                    p =>
                    {
                        DialogueModule solitudeModule = new DialogueModule("To be alone is to bear witness to the world without participating in it. I have seen countless lives pass before me, filled with laughter and sorrow, while I remain an unmoving observer.");
                        solitudeModule.AddOption("Is there no joy in observation?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule joyModule = new DialogueModule("There is a bittersweet beauty in observation, but it cannot replace the warmth of companionship. I long to feel the connection that you mortals share.");
                                joyModule.AddOption("What do you miss the most?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule longingModule = new DialogueModule("I miss the laughter of friends, the embrace of a loved one, and the vibrant colors of a world I can no longer touch. Each season brings reminders of what I once was.");
                                        pla.SendGump(new DialogueGump(pla, longingModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, joyModule));
                            });
                        p.SendGump(new DialogueGump(p, solitudeModule));
                    });

                existenceModule.AddOption("What have you witnessed?",
                    p => true,
                    p =>
                    {
                        DialogueModule witnessModule = new DialogueModule("I have seen kingdoms flourish and fade, heroes rise to glory and fall to ruin, and the endless cycle of life and death. Each story is etched into my memory like markings in stone.");
                        witnessModule.AddOption("Tell me about a hero you admired.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule heroModule = new DialogueModule("Once, there was a warrior named Eldrin, who fought valiantly against the shadows encroaching upon his homeland. His bravery inspired many, but ultimately, he too fell. His spirit, however, lingers in the tales of those who remember him.");
                                pl.SendGump(new DialogueGump(pl, heroModule));
                            });
                        witnessModule.AddOption("What about a fallen kingdom?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule kingdomModule = new DialogueModule("The kingdom of Eldoria, known for its radiant fields and wise rulers, fell to greed and ambition. Their once-flourishing society crumbled under the weight of their desires. I remember their laughter and their pain.");
                                pl.SendGump(new DialogueGump(pl, kingdomModule));
                            });
                        p.SendGump(new DialogueGump(p, witnessModule));
                    });

                player.SendGump(new DialogueGump(player, existenceModule));
            });

        greeting.AddOption("Do you have any knowledge to share?",
            player => true,
            player =>
            {
                DialogueModule knowledgeModule = new DialogueModule("I possess the wisdom of ages. I have seen civilizations rise and fall, and I can impart the lessons I have learned. What knowledge do you seek?");
                knowledgeModule.AddOption("What is the greatest lesson you've learned?",
                    p => true,
                    p =>
                    {
                        DialogueModule lessonModule = new DialogueModule("The greatest lesson is that all things are transient. Nothing lasts forever, and embracing change is essential. To hold on too tightly can lead to despair.");
                        lessonModule.AddOption("How do you cope with change?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule copeModule = new DialogueModule("I have learned to accept change as part of existence. Each season brings new beauty and new challenges. I find solace in the knowledge that I am part of a greater cycle.");
                                pl.SendGump(new DialogueGump(pl, copeModule));
                            });
                        p.SendGump(new DialogueGump(p, lessonModule));
                    });
                knowledgeModule.AddOption("What do you know about the stars?",
                    p => true,
                    p =>
                    {
                        DialogueModule starsModule = new DialogueModule("The stars are the souls of those who have passed, watching over the living. They guide wanderers and inspire dreams. I often ponder the stories they tell to those who gaze upon them.");
                        starsModule.AddOption("Do you believe in fate?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule fateModule = new DialogueModule("Fate is a complex tapestry woven from choices and chance. While I believe we can shape our paths, some threads are beyond our control. Yet, every choice shapes the journey.");
                                pl.SendGump(new DialogueGump(pl, fateModule));
                            });
                        p.SendGump(new DialogueGump(p, starsModule));
                    });
                player.SendGump(new DialogueGump(player, knowledgeModule));
            });

        greeting.AddOption("What is it like to live forever?",
            player => true,
            player =>
            {
                DialogueModule immortalModule = new DialogueModule("To live forever is both a blessing and a curse. I have seen the beauty of countless dawns, but I have also witnessed the slow decay of memories and the loss of companions.");
                immortalModule.AddOption("Is there no escape from this curse?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule escapeModule = new DialogueModule("Escape is a fleeting dream. I have tried to break my bonds, yet every attempt has led me back to this form. Perhaps one day, I will find a way to regain my humanity.");
                        pl.SendGump(new DialogueGump(pl, escapeModule));
                    });
                immortalModule.AddOption("Do you ever feel hope?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule hopeModule = new DialogueModule("Hope flickers like a distant star. It may seem faint, but it is what keeps my heart beating beneath this stone exterior. I hold onto the dream that one day, I may be free.");
                        pl.SendGump(new DialogueGump(pl, hopeModule));
                    });
                player.SendGump(new DialogueGump(player, immortalModule));
            });

        greeting.AddOption("Do you have any gifts for me?",
            player => true,
            player =>
            {
                DialogueModule giftModule = new DialogueModule("I can offer you wisdom and guidance, but material gifts are scarce. However, I do have this gem, infused with the earth's memories. Use it wisely.");
                giftModule.AddOption("What is the gem for?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule gemModule = new DialogueModule("This gem holds fragments of the past and can guide you in times of need. It carries the weight of countless stories, a reminder of the lessons learned through ages.");
                        pl.SendGump(new DialogueGump(pl, gemModule));
                    });
                giftModule.AddOption("Can I receive it now?",
                    pl => true,
                    pl =>
                    {
                        if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                        {
                            pl.SendMessage("I have no reward right now. Please return later.");
                        }
                        else
                        {
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            pl.AddToBackpack(new PoisonHitAreaCrystal()); // Give the reward
                            pl.SendMessage("You have received a gem containing the memories of the earth.");
                        }
                    });
                player.SendGump(new DialogueGump(player, giftModule));
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
