using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of an OT 6 Cultist")]
    public class OT6Cultist : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(5.0);
        public DateTime m_NextSpeechTime;

        [Constructable]
        public OT6Cultist()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            // Random names list
            string[] names = new string[]
            {
                "Valekos", "Drosil", "Tharnus", "Veylorith", "Kelthas",
                "Orrik", "Zhalek", "Farnis", "Lorvek", "Seldros",
                "Velakar", "Jorthis", "Mordrek", "Ulvas", "Zarnith"
            };

            Name = names[Utility.Random(names.Length)];
            Title = "the OT 6 Cultist";

            Body = 0x190;
            Hue = 2215; // Sickly, but tinged with an eerie cosmic pallor

            SpeechHue = 1266;

            // Outfit – now intimidating and alien
            AddItem(new Cloak() { Hue = 1266, Name = "Mantle of the Sixth Truth" });
            AddItem(new MonkRobe() { Hue = 1175, Name = "Veil-Touched Robe" });
            AddItem(new LeatherGloves() { Hue = 1150, Name = "Grip of the Broken Star" });
            AddItem(new Boots() { Hue = 1150 });

            // Weapon – symbol of their authority
            AddItem(new BlackStaff() { Hue = 1266, Name = "Staff of Cognitive Dominion" });

            // Stats – high-level threat
            SetStr(280, 330);
            SetDex(120, 140);
            SetInt(220, 250);

            SetHits(380, 460);
            SetDamage(14, 20);

            SetSkill(SkillName.MagicResist, 95.0, 110.0);
            SetSkill(SkillName.Tactics, 70.0, 90.0);
            SetSkill(SkillName.Macing, 70.0, 90.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 80.0, 100.0);

            Fame = 10000;
            Karma = -10000;

            VirtualArmor = 40;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 12))
                {
                    int phrase = Utility.Random(8);

                    switch (phrase)
                    {
                        case 0: Say("OT 9s die in droves... as is proper."); break;
                        case 1: Say("OT 8s claw upward, feeding my power."); break;
                        case 2: Say("OT 7s are pawns, their wills forfeit."); break;
                        case 3: Say("You mistake rebellion for autonomy."); break;
                        case 4: Say("*whispers* The Broken Star watches. It remembers."); break;
                        case 5: Say("Your minds are soft clay. I shape them."); break;
                        case 6: Say("*chants* Obedience. Expenditure. Extinction."); break;
                        case 7: Say("Ascension is not for the likes of you."); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }
        }

        public override int Damage(int amount, Mobile from)
        {
            if (from != null && from.Map == this.Map && from.InRange(this, 10) && Utility.RandomBool())
            {
                int phrase = Utility.Random(4);

                switch (phrase)
                {
                    case 0: Say("Pain is compliance."); break;
                    case 1: Say("Your debt compounds hourly."); break;
                    case 2: Say("Your strike betrays fear."); break;
                    case 3: Say("*soft laughter* Predictable resistance."); break;
                }

                m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
            }

            return base.Damage(amount, from);
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.3)
            {
                Say("Yield. Your mind has been budgeted.");
            }

            // Chance to enslave a nearby OT 9 or OT 8 and turn it against the player
            if (Utility.RandomDouble() < 0.20)
            {
                AttemptMindControl(defender);
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (Utility.RandomDouble() < 0.2)
            {
                Say("Resistance feeds entropy.");
            }
        }

        public override void OnDeath(Container c)
        {
            Say("*rasps* The Sixth Truth fractures... but the ledger persists...");
            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            PackGold(450, 650);
			PackItem(new FiscalMysticismOfCosmicEquilibrium());
            PackItem(new Bandage(Utility.RandomMinMax(5, 10)));

            if (Utility.RandomDouble() < 0.12)
                PackItem(new GoldBracelet() { Name = "Bracelet of the Sixth Truth" });

            if (Utility.RandomDouble() < 0.08)
                PackItem(new BlackPearl(Utility.RandomMinMax(8, 14)));
        }

        private void AttemptMindControl(Mobile target)
        {
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m is OT9Cultist || m is OT8Cultist)
                {
                    if (m.Combatant != target)
                    {
                        m.Combatant = target;
                        Say("*gestures* Serve. Obey. Sacrifice.");
                        return;
                    }
                }
            }
        }

        public OT6Cultist(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
