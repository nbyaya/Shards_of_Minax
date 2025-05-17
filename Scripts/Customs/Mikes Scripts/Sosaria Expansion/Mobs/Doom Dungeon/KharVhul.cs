using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of Khar'Vhul, the Weighted Axis")]
    public class KharVhul : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(3.0);
        public DateTime m_NextSpeechTime;

        [Constructable]
        public KharVhul()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.1, 0.2)
        {
            Name = "Khar'Vhul";
            Title = "the Weighted Axis - OT3";

            Body = 0x190;
            Hue = 2301; // Pale cosmic gray with faint geometric shifting skin

            SpeechHue = 1152;

            // Outfit – barely decorative, robes marked with formulaic runes
            AddItem(new Cloak() { Hue = 1152, Name = "Axis Mantle of OT 3" });
            AddItem(new Robe() { Hue = 1152, Name = "Runed Vestment of Correction" });
            AddItem(new LeatherGloves() { Hue = 1152, Name = "Hands of Balance" });
            AddItem(new Sandals() { Hue = 1152 });

            // Weapon – none. His *presence* is his weapon.

            // Stats – very high-level boss
            SetStr(360, 420);
            SetDex(150, 170);
            SetInt(360, 400);

            SetHits(750, 900);
            SetDamage(24, 30);

            SetSkill(SkillName.MagicResist, 130.0, 140.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Magery, 135.0, 150.0);
            SetSkill(SkillName.EvalInt, 135.0, 150.0);
            SetSkill(SkillName.Meditation, 120.0, 130.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 60;

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
                    int phrase = Utility.Random(6);

                    switch (phrase)
                    {
                        case 0: Say("3.14159."); break;
                        case 1: Say("Negative mass detected."); break;
                        case 2: Say("Correction vector established."); break;
                        case 3: Say("Balance: unsustainable. Solution: annihilation."); break;
                        case 4: Say("Expenditure exceeds income. Liquidation required."); break;
                        case 5: Say("*A cold voice intones numeric sequences.*"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }
        }

        public override int Damage(int amount, Mobile from)
        {
            if (from != null && from.Map == this.Map && from.InRange(this, 10) && Utility.RandomBool())
            {
                int phrase = Utility.Random(3);

                switch (phrase)
                {
                    case 0: Say("Variable destabilized."); break;
                    case 1: Say("Entropy acknowledged."); break;
                    case 2: Say("Correction subroutine activated."); break;
                }

                m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
            }

            return base.Damage(amount, from);
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.30)
            {
                Say("Rebalance initiated.");
            }

            if (Utility.RandomDouble() < 0.20)
            {
                CorrectionPulse(defender);
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (Utility.RandomDouble() < 0.2)
            {
                Say("Equation adjusting.");
            }
        }

        public override void OnDeath(Container c)
        {
            Say("*coldly* Balance... deferred.");
            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            PackGold(1500, 2000);

            if (Utility.RandomDouble() < 0.15)
                PackItem(new GoldBracelet() { Name = "Bracelet of the Third Vector" });

            if (Utility.RandomDouble() < 0.1)
                PackItem(new BlackPearl(Utility.RandomMinMax(20, 28)));

            if (Utility.RandomDouble() < 0.05)
                PackItem(new GoldRing() { Name = "Khar’Vhul’s Signet of Correction" });
        }

        private void CorrectionPulse(Mobile target)
        {
            if (target != null && target.Alive)
            {
                Say("Correction pulse discharged.");

                target.FixedParticles(0x37C4, 10, 15, 5052, EffectLayer.Head);
                target.PlaySound(0x229);

                // Area debuff pulse
                foreach (Mobile m in GetMobilesInRange(5))
                {
                    if (m == target || m == this || !m.Alive)
                        continue;

                    if (m.Player || m is BaseCreature && ((BaseCreature)m).Controlled)
                    {
                        m.Stam -= 10;
                        m.Mana -= 10;

                        m.SendMessage(1152, "A wave of correction saps your strength.");
                    }
                }
            }
        }

        public KharVhul(Serial serial) : base(serial) { }

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
