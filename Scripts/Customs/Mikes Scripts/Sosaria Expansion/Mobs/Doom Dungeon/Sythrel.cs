using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of Sythrel, the Principled Auditor")]
    public class Sythrel : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(4.0);
        public DateTime m_NextSpeechTime;

        [Constructable]
        public Sythrel()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.1, 0.3)
        {
            Name = "Sythrel";
            Title = "the Principled Auditor - OT4";

            Body = 0x190;
            Hue = 1151; // Pallid white-grey, the color of bone and old paper

            SpeechHue = 1154;

            // Outfit – severe, abstract, minimalistic
            AddItem(new Cloak() { Hue = 1154, Name = "Veil of the Fourth Clause" });
            AddItem(new Robe() { Hue = 1154, Name = "Contractual Vestment of OT 4" });
            AddItem(new LeatherGloves() { Hue = 1154, Name = "Hands of Calculation" });
            AddItem(new Boots() { Hue = 1154 });

            // Weapon – no staff, carries only a cosmic symbol
            AddItem(new GoldRing() { Name = "Signet of Immutable Balance" });

            // Stats – high-level boss
            SetStr(340, 390);
            SetDex(140, 160);
            SetInt(320, 360);

            SetHits(600, 700);
            SetDamage(20, 26);

            SetSkill(SkillName.MagicResist, 120.0, 130.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Magery, 125.0, 140.0);
            SetSkill(SkillName.EvalInt, 125.0, 140.0);
            SetSkill(SkillName.Meditation, 110.0, 120.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 55;

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
                    int phrase = Utility.Random(7);

                    switch (phrase)
                    {
                        case 0: Say("Clause One: All debts must be honored."); break;
                        case 1: Say("Clause Two: Resistance does not alter obligation."); break;
                        case 2: Say("Clause Three: The flesh is collateral."); break;
                        case 3: Say("Clause Four: Termination may be enforced."); break;
                        case 4: Say("You are already accounted for."); break;
                        case 5: Say("Your struggle adjusts nothing."); break;
                        case 6: Say("*quietly* Compliance is axiomatic."); break;
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
                    case 0: Say("Pain: accepted collateral."); break;
                    case 1: Say("Your defiance has no bearing."); break;
                    case 2: Say("Clause Three remains in effect."); break;
                }

                m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
            }

            return base.Damage(amount, from);
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.25)
            {
                Say("Liquidating assets.");
            }

            if (Utility.RandomDouble() < 0.20)
            {
                ApplyClauseFour(defender);
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (Utility.RandomDouble() < 0.2)
            {
                Say("Clause Two remains immutable.");
            }
        }

        public override void OnDeath(Container c)
        {
            Say("*quietly* Clauses... remain. Executors will come.");
            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            PackGold(1000, 1500);

            if (Utility.RandomDouble() < 0.15)
                PackItem(new GoldBracelet() { Name = "Bracelet of the Fourth Clause" });

            if (Utility.RandomDouble() < 0.1)
                PackItem(new BlackPearl(Utility.RandomMinMax(15, 22)));

            if (Utility.RandomDouble() < 0.05)
                PackItem(new GoldRing() { Name = "Immutable Signet of Sythrel" });
        }

        private void ApplyClauseFour(Mobile target)
        {
            if (target != null && target.Alive)
            {
                Say("Clause Four invoked.");

                target.FixedParticles(0x376A, 9, 32, 5032, EffectLayer.Head);
                target.PlaySound(0x1F4);

                // Severe debuff - reduces stamina and dex temporarily
                target.Stam -= 10;
                target.Dex -= 5;

                Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
                {
                    if (target != null && !target.Deleted)
                    {
                        target.Stam += 10;
                        target.Dex += 5;
                    }
                });
            }
        }

        public Sythrel(Serial serial) : base(serial) { }

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
