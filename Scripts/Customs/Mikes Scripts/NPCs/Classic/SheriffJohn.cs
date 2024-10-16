using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Sheriff John")]
public class SheriffJohn : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public SheriffJohn() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Sheriff John";
        Body = 0x190; // Human male body

        // Stats
        SetStr(100);
        SetDex(90);
        SetInt(65);
        SetHits(85);

        // Appearance
        AddItem(new LeatherLegs() { Hue = 1160 });
        AddItem(new LeatherChest() { Hue = 1160 });
        AddItem(new WideBrimHat() { Hue = 1158 });
        AddItem(new Boots() { Hue = 1140 });
        AddItem(new BodySash() { Name = "Town's Badge" });
        AddItem(new FireballWand() { Name = "John's Revolver" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
        
        lastRewardTime = DateTime.MinValue;
    }

    public SheriffJohn(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("What do you want, stranger?");

        greeting.AddOption("Tell me about your job.",
            player => true,
            player => {
                DialogueModule jobModule = new DialogueModule("I'm the law around these parts, and I ain't too thrilled about it.");
                jobModule.AddOption("What about Robin Hood?",
                    pl => true,
                    pl => {
                        DialogueModule robinModule = new DialogueModule("Ah, that scoundrel! Always robbing the rich to feed the poor, but he's just a common thief in my eyes. I swear, if I get my hands on him, I'll bring him to justice!");
                        robinModule.AddOption("What’s so bad about him?",
                            p => true,
                            p => {
                                DialogueModule badModule = new DialogueModule("He thinks he's some kind of hero, but he’s only causing chaos. He's made a mockery of the law and emboldened the outlaws.");
                                badModule.AddOption("What has he done to you?",
                                    p2 => true,
                                    p2 => {
                                        DialogueModule doneModule = new DialogueModule("I’ve lost more than I can count because of him. He steals from honest folk and makes my job ten times harder. He’s a thorn in my side, and I won’t rest until I catch him!");
                                        doneModule.AddOption("How do you plan to catch him?",
                                            p3 => true,
                                            p3 => {
                                                DialogueModule planModule = new DialogueModule("I’ve tracked him to a portal that leads to another world. I’m determined to follow him and put an end to his meddling once and for all!");
                                                planModule.AddOption("What’s on the other side?",
                                                    p4 => true,
                                                    p4 => {
                                                        DialogueModule otherSideModule = new DialogueModule("Who knows? Tales speak of strange creatures and powerful magic. But I don’t care; I’ll chase that scoundrel wherever he runs.");
                                                        otherSideModule.AddOption("What if you can’t catch him?",
                                                            p5 => true,
                                                            p5 => {
                                                                DialogueModule notCatchModule = new DialogueModule("I refuse to think that way! I’ve come too far to give up now. If I have to face down a dragon or a sorcerer, I will.");
                                                                player.SendGump(new DialogueGump(player, notCatchModule));
                                                            });
                                                        p4.SendGump(new DialogueGump(p4, otherSideModule));
                                                    });
                                                p3.SendGump(new DialogueGump(p3, planModule));
                                            });
                                        p2.SendGump(new DialogueGump(p2, doneModule));
                                    });
                                p.SendGump(new DialogueGump(p, badModule));
                            });
                        pl.SendGump(new DialogueGump(pl, robinModule));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("Do you think the law is just?",
            player => true,
            player => {
                DialogueModule lawModule = new DialogueModule("Justice can be a fickle thing. Some believe that Robin Hood is a hero, but in my eyes, he’s just a criminal who defies the law.");
                lawModule.AddOption("What would you do if you could change the law?",
                    pl => true,
                    pl => {
                        DialogueModule changeModule = new DialogueModule("I would make it clear that no one is above the law, not even those who think they’re doing good. There needs to be consequences for their actions.");
                        player.SendGump(new DialogueGump(player, changeModule));
                    });
                player.SendGump(new DialogueGump(player, lawModule));
            });

        greeting.AddOption("What do you know about the portal?",
            player => true,
            player => {
                DialogueModule portalModule = new DialogueModule("It's a gateway to another realm, hidden deep within the forest. I heard whispers of it from travelers who sought refuge.");
                portalModule.AddOption("What will you do when you find it?",
                    pl => true,
                    pl => {
                        DialogueModule findModule = new DialogueModule("I'll charge right through and confront him. No more running; no more hiding! He’ll have to face the music.");
                        player.SendGump(new DialogueGump(player, findModule));
                    });
                player.SendGump(new DialogueGump(player, portalModule));
            });

        greeting.AddOption("What do you think of outlaws?",
            player => true,
            player => {
                DialogueModule outlawsModule = new DialogueModule("They're nothing but trouble! If it weren't for Robin Hood's antics, I could focus on keeping the peace. They see him as a role model, and that’s the worst part.");
                outlawsModule.AddOption("Have you faced any tough outlaws?",
                    pl => true,
                    pl => {
                        DialogueModule toughModule = new DialogueModule("Oh, plenty! But none compare to Robin. He’s like a ghost, always slipping through my fingers. I’ve had my share of run-ins, but they pale in comparison.");
                        player.SendGump(new DialogueGump(player, toughModule));
                    });
                player.SendGump(new DialogueGump(player, outlawsModule));
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
