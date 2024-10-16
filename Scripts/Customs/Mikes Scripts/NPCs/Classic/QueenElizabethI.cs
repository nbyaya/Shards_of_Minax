using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Queen Elizabeth I")]
public class QueenElizabethI : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public QueenElizabethI() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Queen Elizabeth I";
        Body = 0x191; // Human female body

        // Stats
        SetStr(90);
        SetDex(70);
        SetInt(100);
        SetHits(70);

        // Appearance
        AddItem(new FancyDress() { Hue = 1152 }); // Fancy dress with hue 1152
        AddItem(new GoldNecklace());
        AddItem(new Boots() { Hue = 1175 }); // Boots with hue 1175
        AddItem(new Spellbook() { Name = "Queen Elizabeth's Journal" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        lastRewardTime = DateTime.MinValue; // Initialize last reward time
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
        DialogueModule greeting = new DialogueModule("I am Queen Elizabeth I of England, the sovereign of a mighty kingdom! How may I assist you?");

        greeting.AddOption("Tell me about your struggles to gain the crown.",
            player => true,
            player => 
            {
                DialogueModule crownStruggleModule = new DialogueModule("Ah, the path to the throne was fraught with peril. My rise began with the tumultuous reign of my father, Henry VIII, and the political machinations that followed his death.");
                crownStruggleModule.AddOption("What challenges did you face?",
                    p => true,
                    p =>
                    {
                        DialogueModule challengesModule = new DialogueModule("I faced many challenges, including being declared illegitimate and the subsequent reigns of my half-siblings. Each sought to undermine my claim to the throne.");
                        challengesModule.AddOption("How did you overcome those obstacles?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule overcomeModule = new DialogueModule("I learned to navigate the treacherous waters of court politics, forming alliances and earning the loyalty of those around me.");
                                overcomeModule.AddOption("What alliances did you form?",
                                    p1 => true,
                                    p1 =>
                                    {
                                        DialogueModule alliancesModule = new DialogueModule("I allied with powerful nobles and courtiers, such as Sir William Cecil, who became one of my most trusted advisors. Their support was crucial for my ascent.");
                                        alliancesModule.AddOption("Tell me more about Sir William Cecil.",
                                            p2 => true,
                                            p2 =>
                                            {
                                                DialogueModule cecilModule = new DialogueModule("Sir William Cecil was a brilliant strategist and a steadfast supporter. His counsel was invaluable, guiding my decisions during perilous times.");
                                                p2.SendGump(new DialogueGump(p2, cecilModule));
                                            });
                                        p1.SendGump(new DialogueGump(p1, alliancesModule));
                                    });
                                p.SendGump(new DialogueGump(p, overcomeModule));
                            });
                        p.SendGump(new DialogueGump(p, challengesModule));
                    });
                player.SendGump(new DialogueGump(player, crownStruggleModule));
            });

        greeting.AddOption("What was your greatest challenge as queen?",
            player => true,
            player => 
            {
                DialogueModule greatestChallengeModule = new DialogueModule("My greatest challenge was to maintain stability in England amid threats both external and internal. The Spanish Armada sought to overthrow me, and plots from within the court constantly loomed.");
                greatestChallengeModule.AddOption("How did you deal with the Spanish Armada?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule armadaModule = new DialogueModule("The defeat of the Armada was a defining moment for my reign. With the help of my brave seamen and the weather's favor, we turned the tide against a formidable foe.");
                        armadaModule.AddOption("What did you learn from that experience?",
                            p => true,
                            p =>
                            {
                                DialogueModule learnModule = new DialogueModule("I learned the importance of unity and resilience. The courage of my people inspired me to strengthen our defenses and build a formidable navy.");
                                p.SendGump(new DialogueGump(p, learnModule));
                            });
                        pl.SendGump(new DialogueGump(pl, armadaModule));
                    });
                player.SendGump(new DialogueGump(player, greatestChallengeModule));
            });

        greeting.AddOption("What do you think about justice?",
            player => true,
            player => 
            {
                DialogueModule justiceModule = new DialogueModule("The virtue of justice is of utmost importance in my reign. Without it, chaos would prevail.");
                justiceModule.AddOption("How do you ensure justice in your kingdom?",
                    p => true,
                    p =>
                    {
                        DialogueModule ensureJusticeModule = new DialogueModule("I seek counsel from wise advisors and ensure fair trials for my subjects. Justice strengthens the bond between the crown and the people.");
                        p.SendGump(new DialogueGump(p, ensureJusticeModule));
                    });
                player.SendGump(new DialogueGump(player, justiceModule));
            });

        greeting.AddOption("Tell me about your kingdom.",
            player => true,
            player => 
            {
                DialogueModule kingdomModule = new DialogueModule("My kingdom of England has a rich history and has seen many challenges. Yet, with the unity of our people, we always persevere.");
                kingdomModule.AddOption("What are your hopes for England's future?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule hopesModule = new DialogueModule("I hope for a prosperous future, where our arts flourish and our people live in peace. Unity is our strength.");
                        pl.SendGump(new DialogueGump(pl, hopesModule));
                    });
                player.SendGump(new DialogueGump(player, kingdomModule));
            });

        greeting.AddOption("What can you offer me?",
            player => true,
            player => 
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    DialogueModule rewardCooldownModule = new DialogueModule("I have no reward right now. Please return later.");
                    player.SendGump(new DialogueGump(player, rewardCooldownModule));
                }
                else
                {
                    DialogueModule rewardModule = new DialogueModule("For your dedication to the pursuit of justice, I reward you.");
                    player.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
            });

        return greeting;
    }

    public QueenElizabethI(Serial serial) : base(serial) { }

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
