using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using System.Collections;

namespace Server.Mobiles
{
    [CorpseName("a chaos wyrm corpse")]
    public class ChaosWyrm : BaseCreature
    {
        private bool m_ImmunePhaseActive;
        private DateTime m_NextAoE;
        private DateTime m_NextConfusion;

        [Constructable]
        public ChaosWyrm() : base(AIType.AI_Mage, FightMode.Weakest | FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Chaos Wyrm";
            Body = 46; // Use an appropriate body ID for the boss
            BaseSoundID = 362;

            SetStr(800, 1000);
            SetDex(150, 200);
            SetInt(600, 750);

            SetHits(10000);
            SetStam(250);
            SetMana(5000);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 60, 80);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 150.0, 180.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 80.0, 100.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

            m_ImmunePhaseActive = false;
            m_NextAoE = DateTime.Now + TimeSpan.FromSeconds(20);
            m_NextConfusion = DateTime.Now + TimeSpan.FromSeconds(30);
        }

        public override void OnThink()
        {
            base.OnThink();

            // Area of Effect (AoE) Damage every 20 seconds
            if (DateTime.Now >= m_NextAoE)
            {
                DoAreaEffectDamage();
                m_NextAoE = DateTime.Now + TimeSpan.FromSeconds(20);
            }

            // Confusion ability every 30 seconds
            if (DateTime.Now >= m_NextConfusion)
            {
                DoConfusionAttack();
                m_NextConfusion = DateTime.Now + TimeSpan.FromSeconds(30);
            }

            // Immune Phase activation below 50% health
            if (!m_ImmunePhaseActive && Hits < (HitsMax / 2))
            {
                ActivateImmunePhase();
            }
        }

        private void DoAreaEffectDamage()
        {
            Map map = this.Map;

            if (map == null)
                return;

            ArrayList targets = new ArrayList();

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && CanBeHarmful(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];
                DoHarmful(m);

                int damage = Utility.RandomMinMax(20, 40); // Adjust damage as needed

                m.Damage(damage, this);
                m.SendMessage("You are scorched by the Chaos Wyrm's fiery breath!");
            }
        }

        private void DoConfusionAttack()
        {
            Map map = this.Map;

            if (map == null)
                return;

            ArrayList targets = new ArrayList();

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && CanBeHarmful(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];
                DoHarmful(m);

                m.Paralyzed = true; // Freeze them in place as confusion takes hold
                m.SendMessage("You are overwhelmed by a wave of confusion!");

                Timer.DelayCall(TimeSpan.FromSeconds(5.0), delegate { m.Paralyzed = false; });
            }
        }

        private void ActivateImmunePhase()
        {
            m_ImmunePhaseActive = true;
            this.Say("You cannot harm me during my shield phase!");

            Timer.DelayCall(TimeSpan.FromSeconds(10.0), delegate
            {
                m_ImmunePhaseActive = false;
                this.Say("My shield fades, and I am vulnerable once more!");
            });
        }

        public override bool OnBeforeDeath()
        {
            // Optional: Add final phase behavior, such as removing the immune phase.
            return base.OnBeforeDeath();
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            // Bleed effect on melee hit
            if (0.25 >= Utility.RandomDouble()) // 25% chance to inflict bleed
            {
                defender.SendMessage("You have been badly wounded and start to bleed!");

                defender.PlaySound(0x133); // Sound effect for bleeding

                // Apply the Bleed effect
                defender.BeginAction(typeof(ChaosWyrm));
                Timer.DelayCall(TimeSpan.FromSeconds(10.0), delegate { defender.EndAction(typeof(ChaosWyrm)); });

                Timer.DelayCall(TimeSpan.FromSeconds(2.0), TimeSpan.FromSeconds(2.0), 5, () =>
                {
                    if (defender.Alive)
                    {
                        int bleedDamage = Utility.RandomMinMax(3, 8); // Adjust damage as needed
                        defender.Damage(bleedDamage, this);
                        defender.SendMessage("You continue to bleed!");
                    }
                });
            }
        }

        public ChaosWyrm(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
