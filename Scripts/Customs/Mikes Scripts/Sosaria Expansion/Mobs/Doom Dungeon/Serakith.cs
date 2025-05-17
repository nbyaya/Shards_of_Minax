using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of Serakith, the Final Executor")]
    public class Serakith : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(2.5);
        public DateTime m_NextSpeechTime;

        [Constructable]
        public Serakith()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.05, 0.1)
        {
            Name = "Serakith";
            Title = "the Final Executor OT1";

            Body = 0x190;
            Hue = 1157; // Ethereal gray-blue, almost translucent

            SpeechHue = 1150;

            // Outfit – barely there. A wraithlike presence.
            AddItem(new Cloak() { Hue = 1150, Name = "Mantle of the Closing Ledger" });
            AddItem(new DeathRobe() { Hue = 1150, Name = "Vestment of the Final Clause" });

            // No gloves. No shoes. No jewelry. Identity erased.

            // Stats – raid boss level
            SetStr(450, 500);
            SetDex(180, 200);
            SetInt(500, 550);

            SetHits(1200, 1400);
            SetDamage(28, 34);

            SetSkill(SkillName.MagicResist, 150.0, 160.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Magery, 160.0, 175.0);
            SetSkill(SkillName.EvalInt, 160.0, 175.0);
            SetSkill(SkillName.Meditation, 140.0, 150.0);

            Fame = 50000;
            Karma = -50000;

            VirtualArmor = 70;

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

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 16))
                {
                    int phrase = Utility.Random(6);

                    switch (phrase)
                    {
                        case 0: Say("Judged."); break;
                        case 1: Say("Irredeemable."); break;
                        case 2: Say("Clause final."); break;
                        case 3: Say("Null value."); break;
                        case 4: Say("Balance... enforced."); break;
                        case 5: Say("*a voice without origin* Termination authorized."); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }
        }

        public override int Damage(int amount, Mobile from)
        {
            if (from != null && from.Map == this.Map && from.InRange(this, 12) && Utility.RandomBool())
            {
                Say("*writes an invisible final tally*");
            }

            return base.Damage(amount, from);
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.35)
            {
                Say("Ledger closed.");
            }

            if (Utility.RandomDouble() < 0.25)
            {
                ApplyFinalClause(defender);
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (Utility.RandomDouble() < 0.15)
            {
                Say("Resistance: invalid.");
            }
        }

        public override void OnDeath(Container c)
        {
            Say("*a terrible stillness fills the air* Enforcement... concluded. Convergence... imminent.");
            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            PackGold(2500, 3000);

            if (Utility.RandomDouble() < 0.2)
                PackItem(new GoldBracelet() { Name = "Bracelet of the Final Clause" });

            if (Utility.RandomDouble() < 0.1)
                PackItem(new BlackPearl(Utility.RandomMinMax(30, 40)));

            if (Utility.RandomDouble() < 0.05)
                PackItem(new GoldRing() { Name = "Executor’s Seal of Serakith" });
        }

        private void ApplyFinalClause(Mobile target)
        {
            if (target != null && target.Alive)
            {
                Say("Final clause enacted.");

                target.FixedParticles(0x37CB, 10, 15, 5052, EffectLayer.Head);
                target.PlaySound(0x1FE);

                // Crushing debuff: reduces strength, dexterity, and intelligence temporarily
                target.Str -= 5;
                target.Dex -= 5;
                target.Int -= 5;

                target.SendMessage(1150, "A terrible judgment weighs upon you.");

                Timer.DelayCall(TimeSpan.FromSeconds(45), () =>
                {
                    if (target != null && !target.Deleted)
                    {
                        target.Str += 5;
                        target.Dex += 5;
                        target.Int += 5;
                    }
                });
            }
        }

        public Serakith(Serial serial) : base(serial) { }

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
