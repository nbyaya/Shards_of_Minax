using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("corpse of Draconicus the Destroyer")]
    public class DraconicusTheDestroyer : BaseCreature
    {
        private DateTime _nextSpecialAbility;
        private int _phase = 1;

        [Constructable]
        public DraconicusTheDestroyer() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Draconicus the Destroyer";
            Body = 12; // Dragon
            BaseSoundID = 362;

            SetStr(1000);
            SetDex(200);
            SetInt(1000);

            SetHits(30000);
            SetMana(5000);

            SetDamage(50, 80);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 50);

            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 100);
            SetResistance(ResistanceType.Cold, 50);
            SetResistance(ResistanceType.Poison, 60);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 60;

            _nextSpecialAbility = DateTime.UtcNow;
        }

        public DraconicusTheDestroyer(Serial serial) : base(serial)
        {
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool Unprovokable { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null)
                return;

            if (DateTime.UtcNow >= _nextSpecialAbility)
            {
                UseSpecialAbility();
                _nextSpecialAbility = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            }

            // Phase transition
            if (_phase == 1 && Hits < HitsMax * 0.5)
            {
                _phase = 2;
                Say("You fool! This isn't even my final form!");
                Body = 59; // Changing to another form (e.g., Titan)
                Hue = 1175; // Fiery red hue
            }
        }

        private void UseSpecialAbility()
        {
            int ability = Utility.Random(5);
            switch (ability)
            {
                case 0:
                    AreaAttack();
                    break;
                case 1:
                    SummonMinions();
                    break;
                case 2:
                    Teleport();
                    break;
                case 3:
                    InflictStatusAilment();
                    break;
                case 4:
                    DrainLife();
                    break;
            }
        }

        private void AreaAttack()
        {
            Say("Feel the wrath of my flames!");
            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m is PlayerMobile && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(80, 120), 0, 100, 0, 0, 0);
                    m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                }
            }
        }

        private void SummonMinions()
        {
            Say("Rise, my minions!");
            for (int i = 0; i < 3; i++)
            {
                BaseCreature minion = new Dragon();
                minion.MoveToWorld(Location, Map);
                minion.Combatant = Combatant;
            }
        }

        private void Teleport()
        {
            Say("You cannot escape me!");
            Map map = Map;
            if (map == null)
                return;

            int x = X + Utility.RandomMinMax(-10, 10);
            int y = Y + Utility.RandomMinMax(-10, 10);
            int z = map.GetAverageZ(x, y);

            Point3D loc = new Point3D(x, y, z);

            MoveToWorld(loc, map);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
            Effects.SendLocationParticles(EffectItem.Create(loc, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);
        }

        private void InflictStatusAilment()
        {
            if (Combatant is PlayerMobile)
            {
                PlayerMobile target = (PlayerMobile)Combatant;
                int randomEffect = Utility.Random(3);
                switch (randomEffect)
                {
                    case 0:
                        target.ApplyPoison(this, Poison.Deadly);
                        target.SendLocalizedMessage(1010512); // You have been poisoned!
                        break;
                    case 1:
                        target.Paralyze(TimeSpan.FromSeconds(5));
                        target.SendLocalizedMessage(1005603); // You can't move!
                        break;
                    case 2:
                        SpellHelper.AddStatCurse(this, target, StatType.All, 50, TimeSpan.FromSeconds(30));
                        target.SendLocalizedMessage(1075091); // You are overcome by a sense of weakness.
                        break;
                }
            }
        }

        private void DrainLife()
        {
            Say("Your life force sustains me!");
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m is PlayerMobile && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    int damage = Utility.RandomMinMax(40, 80);
                    AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);
                    Hits += damage;
                    m.FixedParticles(0x374A, 10, 15, 5013, EffectLayer.Waist);
                }
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 5);
            AddLoot(LootPack.Gems, 8);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
            writer.Write(_phase);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            _phase = reader.ReadInt();
        }
    }
}
