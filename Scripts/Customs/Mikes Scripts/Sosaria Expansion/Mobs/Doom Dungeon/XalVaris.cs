using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of Xal'Varis, Auditor of Flesh")]
    public class XalVaris : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(5.0);
        public DateTime m_NextSpeechTime;

        [Constructable]
        public XalVaris()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.1, 0.3)
        {
            Name = "Xal'Varis";
            Title = "the Auditor of Flesh - OT5";

            Body = 0x190;
            Hue = 1150; // Darkened skin, void-tainted

            SpeechHue = 1373;

            // Outfit – regal, terrifying
            AddItem(new Cloak() { Hue = 1373, Name = "Ledger-Veil of OT 5" });
            AddItem(new Robe() { Hue = 1150, Name = "Investiture of the Flesh Auditor" });
            AddItem(new LeatherGloves() { Hue = 1150, Name = "Accountant’s Grasp" });
            AddItem(new Boots() { Hue = 1150 });

            // Weapon – cosmic balance
            AddItem(new BlackStaff() { Hue = 1175, Name = "Staff of Flesh Equity" });

            // Stats – serious boss level
            SetStr(320, 380);
            SetDex(140, 160);
            SetInt(280, 320);

            SetHits(520, 600);
            SetDamage(18, 24);

            SetSkill(SkillName.MagicResist, 110.0, 125.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.Macing, 80.0, 100.0);
            SetSkill(SkillName.Magery, 115.0, 130.0);
            SetSkill(SkillName.EvalInt, 115.0, 130.0);
            SetSkill(SkillName.Meditation, 90.0, 110.0);

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 50;

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
                        case 0: Say("Your debt cannot be waived."); break;
                        case 1: Say("Pain. The only true currency."); break;
                        case 2: Say("Your worth will be *audited* in blood."); break;
                        case 3: Say("OT 6 prepared you poorly."); break;
                        case 4: Say("*writes invisible numbers in the air* You are... deficient."); break;
                        case 5: Say("No refunds. Only recompense."); break;
                        case 6: Say("Surrender. It reduces penalties."); break;
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
                    case 0: Say("Your strike accrues interest."); break;
                    case 1: Say("Damage noted. Charges pending."); break;
                    case 2: Say("*marks the air* Penalty applied."); break;
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
                Say("This wound is tax-deductible... for *me*.");
            }

            // 20% chance to "Invoice" the player: small debuff applied
            if (Utility.RandomDouble() < 0.20)
            {
                InvoiceDebuff(defender);
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (Utility.RandomDouble() < 0.2)
            {
                Say("Defiance noted for collection.");
            }
        }

        public override void OnDeath(Container c)
        {
            Say("*hisses* The ledger... never... balances...");
            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            PackGold(800, 1200);

            if (Utility.RandomDouble() < 0.15)
                PackItem(new GoldBracelet() { Name = "Bracelet of the Fifth Ledger" });

            if (Utility.RandomDouble() < 0.10)
                PackItem(new BlackPearl(Utility.RandomMinMax(10, 18)));

            if (Utility.RandomDouble() < 0.05)
                PackItem(new GoldRing() { Name = "Signet of Xal’Varis" });
        }

        private void InvoiceDebuff(Mobile target)
        {
            if (target != null && target.Alive)
            {
                Say("*brands an invisible sigil upon " + target.Name + "*");

                // Simple debuff effect - lowers strength temporarily
                target.FixedParticles(0x375A, 10, 15, 5030, EffectLayer.Head);
                target.PlaySound(0x1ED);

                target.Str -= 5;

                Timer.DelayCall(TimeSpan.FromSeconds(20), () =>
                {
                    if (target != null && !target.Deleted)
                        target.Str += 5;
                });
            }
        }

        public XalVaris(Serial serial) : base(serial) { }

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
