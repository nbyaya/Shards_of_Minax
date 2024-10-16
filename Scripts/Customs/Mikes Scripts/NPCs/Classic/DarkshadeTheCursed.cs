using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class DarkshadeTheCursed : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public DarkshadeTheCursed() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Darkshade the Cursed";
        Body = 0x190; // Human male body

        // Stats
        SetStr(130);
        SetDex(50);
        SetInt(150);
        SetHits(110);

        // Appearance
        AddItem(new Robe(1109)); // Robe with hue 1109
        AddItem(new Sandals(1157)); // Sandals with hue 1157
        AddItem(new SkullCap(1175)); // SkullCap with hue 1175

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public DarkshadeTheCursed(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Ah, a visitor... I am Darkshade the Cursed, a necromancer of great power. What do you seek from me?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("I am Darkshade the Cursed, master of necromancy and wielder of dark arts. My knowledge extends to realms others fear to tread.");
                aboutModule.AddOption("Tell me more about the dark arts.",
                    p => true,
                    p =>
                    {
                        DialogueModule darkArtsModule = new DialogueModule("The dark arts allow one to manipulate life and death, to command the shadows and bind the undead to your will. Few dare to learn my secrets.");
                        darkArtsModule.AddOption("Can you teach me some of these secrets?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule secretModule = new DialogueModule("To prove yourself worthy of my secrets, answer me this: What is the most potent reagent in necromancy?");
                                secretModule.AddOption("Nightshade?",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("Nightshade... a good guess, but not quite. Seek the answer, and perhaps one day, you'll understand.");
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                secretModule.AddOption("Bloodmoss?",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("Bloodmoss has its uses, but it is not the most potent. Keep searching for the truth.");
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                secretModule.AddOption("Grave Dust?",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("Ah, Grave Dust. A crucial component in necromancy, indeed. You show promise, traveler.");
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                secretModule.AddOption("Tell me more about the nature of necromancy.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule natureModule = new DialogueModule("Necromancy is not merely about death. It is about understanding the delicate balance between life, death, and the energies that bind them. There are multiple planes of existence, and necromancy is a bridge to those realms.");
                                        natureModule.AddOption("What are these planes of existence?",
                                            pla2 => true,
                                            pla2 =>
                                            {
                                                DialogueModule planesModule = new DialogueModule("The planes are many, but most mortals know only of the Material Plane. Beyond it lies the Shadow Plane, a realm of perpetual twilight where the souls of the departed linger. There is also the Ethereal Plane, a bridge between the living and the dead, and the Astral Plane, where the spirits of powerful beings roam freely.");
                                                planesModule.AddOption("Tell me about the Shadow Plane.",
                                                    pla3 => true,
                                                    pla3 =>
                                                    {
                                                        DialogueModule shadowModule = new DialogueModule("The Shadow Plane is a dark reflection of our own world. It is where the spirits of the departed often reside when they are unable or unwilling to move on. The very air there is thick with the essence of sorrow and despair, and it is here that necromancers draw much of their power.");
                                                        shadowModule.AddOption("What happens to spirits that remain in the Shadow Plane?",
                                                            pla4 => true,
                                                            pla4 =>
                                                            {
                                                                DialogueModule spiritsModule = new DialogueModule("Spirits that linger in the Shadow Plane can become restless. Some may become shades, bound by regret or unfinished business. Others may be corrupted by the dark energies of the plane, turning into malevolent wraiths. It is the necromancer's role to understand these spirits, to communicate with them, or, if necessary, bind them.");
                                                                spiritsModule.AddOption("Can these spirits be helped?",
                                                                    pla5 => true,
                                                                    pla5 =>
                                                                    {
                                                                        DialogueModule helpSpiritsModule = new DialogueModule("Indeed, some spirits can be helped. By addressing their unresolved attachments, a necromancer can guide them to their final rest. However, it is a delicate process, for meddling too deeply can lead to one's own corruption.");
                                                                        helpSpiritsModule.AddOption("What kind of attachments keep them bound?",
                                                                            pla6 => true,
                                                                            pla6 =>
                                                                            {
                                                                                DialogueModule attachmentsModule = new DialogueModule("Attachments vary. Some spirits are bound by love, others by revenge, and still others by mere confusion. The strongest bindings are those of betrayal, where a spirit cannot move on without some form of justice or redemption.");
                                                                                attachmentsModule.AddOption("How does one offer redemption?",
                                                                                    pla7 => true,
                                                                                    pla7 =>
                                                                                    {
                                                                                        DialogueModule redemptionModule = new DialogueModule("Redemption is not easy to offer. Sometimes it requires fulfilling a promise the spirit made in life, other times it requires confronting those who wronged the spirit. The process is fraught with danger, as the spirit's emotions are powerful enough to affect the living.");
                                                                                        redemptionModule.AddOption("What dangers are involved?",
                                                                                            pla8 => true,
                                                                                            pla8 =>
                                                                                            {
                                                                                                DialogueModule dangerModule = new DialogueModule("The emotions of the dead are potent, and attempting to help them can lead to possession, curses, or worse. A spirit's rage can manifest as physical harm, while their sorrow can drain the life force from the living. Only those with strong wills and pure intentions should dare to intervene.");
                                                                                                dangerModule.AddOption("How can one protect themselves?",
                                                                                                    pla9 => true,
                                                                                                    pla9 =>
                                                                                                    {
                                                                                                        DialogueModule protectModule = new DialogueModule("Protection comes in many forms: wards, sigils, and charms crafted from rare reagents. But the most important protection is one's own intent. A pure heart and focused mind are the strongest defenses against the influence of the dead.");
                                                                                                        protectModule.AddOption("What rare reagents are needed?",
                                                                                                            pla10 => true,
                                                                                                            pla10 =>
                                                                                                            {
                                                                                                                DialogueModule reagentsModule = new DialogueModule("The rarest reagents include Soul Dust, gathered from spirits who willingly moved on, and Ectoplasmic Residue, harvested from encounters with ethereal beings. These items, when combined with focused intent, create a barrier between the necromancer and the malevolent forces.");
                                                                                                                reagentsModule.AddOption("Thank you for this knowledge.",
                                                                                                                    plb => true,
                                                                                                                    plb =>
                                                                                                                    {
                                                                                                                        plb.SendMessage("Darkshade nods subtly, acknowledging your understanding.");
                                                                                                                        plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                                                                                                    });
                                                                                                                pla10.SendGump(new DialogueGump(pla10, reagentsModule));
                                                                                                            });
                                                                                                        pla9.SendGump(new DialogueGump(pla9, protectModule));
                                                                                                    });
                                                                                                pla8.SendGump(new DialogueGump(pla8, dangerModule));
                                                                                            });
                                                                                        pla7.SendGump(new DialogueGump(pla7, redemptionModule));
                                                                                    });
                                                                                pla6.SendGump(new DialogueGump(pla6, attachmentsModule));
                                                                            });
                                                                        pla5.SendGump(new DialogueGump(pla5, helpSpiritsModule));
                                                                    });
                                                                pla4.SendGump(new DialogueGump(pla4, spiritsModule));
                                                            });
                                                        pla3.SendGump(new DialogueGump(pla3, shadowModule));
                                                    });
                                                planesModule.AddOption("Tell me about the Ethereal Plane.",
                                                    pla3 => true,
                                                    pla3 =>
                                                    {
                                                        DialogueModule etherealModule = new DialogueModule("The Ethereal Plane is a border realm, a place of mist and twilight. It is the space between life and death, where souls travel briefly before reaching their final destination. Some beings are capable of traversing the Ethereal Plane, and it is here that ghosts and spirits sometimes linger.");
                                                        etherealModule.AddOption("What kind of beings traverse the Ethereal Plane?",
                                                            pla4 => true,
                                                            pla4 =>
                                                            {
                                                                DialogueModule beingsModule = new DialogueModule("Beings such as phantoms, specters, and even some skilled mages can traverse the Ethereal Plane. The mages do so using spells, while the spirits cross naturally, seeking closure or merely wandering, lost between worlds.");
                                                                beingsModule.AddOption("Can mages help the spirits here?",
                                                                    pla5 => true,
                                                                    pla5 =>
                                                                    {
                                                                        DialogueModule helpModule = new DialogueModule("Yes, skilled mages can help guide spirits, though it is a challenging task. It requires empathy, knowledge of the spirit's past, and an understanding of the risks involved in meddling with the afterlife.");
                                                                        helpModule.AddOption("Thank you for the insight.",
                                                                            plb => true,
                                                                            plb =>
                                                                            {
                                                                                plb.SendMessage("Darkshade inclines his head slightly, acknowledging your gratitude.");
                                                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                                                            });
                                                                        pla5.SendGump(new DialogueGump(pla5, helpModule));
                                                                    });
                                                                pla4.SendGump(new DialogueGump(pla4, beingsModule));
                                                            });
                                                        pla3.SendGump(new DialogueGump(pla3, etherealModule));
                                                    });
                                                planesModule.AddOption("Tell me about the Astral Plane.",
                                                    pla3 => true,
                                                    pla3 =>
                                                    {
                                                        DialogueModule astralModule = new DialogueModule("The Astral Plane is a vast, endless expanse where only the most powerful of beings travel. It is where the consciousness can wander freely, untethered from the body. It is said that powerful deities and ancient spirits make their homes here, and it is dangerous for mortals to enter without proper preparation.");
                                                        astralModule.AddOption("What dangers exist in the Astral Plane?",
                                                            pla4 => true,
                                                            pla4 =>
                                                            {
                                                                DialogueModule dangerAstralModule = new DialogueModule("The Astral Plane is a realm where thought becomes reality. Any fear, doubt, or negative emotion can manifest into a threat. Astral predators, such as mind leeches and void spirits, are always hunting for vulnerable travelers. One must maintain complete control over their thoughts to survive.");
                                                                dangerAstralModule.AddOption("How can one control their thoughts?",
                                                                    pla5 => true,
                                                                    pla5 =>
                                                                    {
                                                                        DialogueModule controlModule = new DialogueModule("Controlling one's thoughts requires immense discipline, meditation, and training. It is said that only those who have achieved true inner peace can traverse the Astral Plane safely. Techniques like mantra repetition and visualization are often used to maintain focus.");
                                                                        controlModule.AddOption("Thank you for the knowledge.",
                                                                            plb => true,
                                                                            plb =>
                                                                            {
                                                                                plb.SendMessage("Darkshade smiles enigmatically, acknowledging your determination to learn.");
                                                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                                                            });
                                                                        pla5.SendGump(new DialogueGump(pla5, controlModule));
                                                                    });
                                                                pla4.SendGump(new DialogueGump(pla4, dangerAstralModule));
                                                            });
                                                        pla3.SendGump(new DialogueGump(pla3, astralModule));
                                                    });
                                                planesModule.AddOption("Thank you for enlightening me.",
                                                    pla3 => true,
                                                    pla3 =>
                                                    {
                                                        pla3.SendMessage("Darkshade inclines his head, his eyes reflecting the knowledge of many planes.");
                                                        pla3.SendGump(new DialogueGump(pla3, CreateGreetingModule()));
                                                    });
                                                pla2.SendGump(new DialogueGump(pla2, planesModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, natureModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, secretModule));
                            });
                        darkArtsModule.AddOption("I am not interested in dark magic.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, darkArtsModule));
                    });
                aboutModule.AddOption("I have heard enough.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("What is your purpose here?",
            player => true,
            player =>
            {
                DialogueModule purposeModule = new DialogueModule("My purpose? To delve into the dark arts, to unlock the secrets of life and death, and to control the undead. I seek the hidden mantras that grant ultimate power.");
                purposeModule.AddOption("Tell me about these mantras.",
                    p => true,
                    p =>
                    {
                        DialogueModule mantraModule = new DialogueModule("The mantra of Honesty is one such powerful incantation. I know but a piece: the third syllable is FOD. Remember it, for it may serve you well.");
                        mantraModule.AddOption("Thank you for sharing this knowledge.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Darkshade nods subtly, acknowledging your understanding.");
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, mantraModule));
                    });
                purposeModule.AddOption("I see, I shall take my leave.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, purposeModule));
            });

        greeting.AddOption("Do you control the undead?",
            player => true,
            player =>
            {
                DialogueModule undeadModule = new DialogueModule("Indeed, the undead serve me willingly. I know the words that bind them, words rooted in deep truths much like the mantra of Honesty.");
                undeadModule.AddOption("What is the mantra of Honesty?",
                    p => true,
                    p =>
                    {
                        DialogueModule honestyModule = new DialogueModule("Honesty is a virtue that binds the spirit and brings clarity. The third syllable is FOD, a small part of a greater whole.");
                        honestyModule.AddOption("Thank you for the knowledge.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Darkshade smiles enigmatically as you ponder his words.");
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, honestyModule));
                    });
                undeadModule.AddOption("I have no interest in controlling the undead.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, undeadModule));
            });

        greeting.AddOption("Farewell, Darkshade.",
            player => true,
            player =>
            {
                player.SendMessage("Darkshade inclines his head slightly as you depart.");
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