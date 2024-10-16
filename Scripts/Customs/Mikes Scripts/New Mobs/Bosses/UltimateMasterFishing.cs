using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Jacques Cousteau")]
    public class UltimateMasterFishing : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterFishing()
            : base(AIType.AI_Mage)
        {
            Name = "Jacques Cousteau";
            Title = "The Oceanographer";
            Body = 0x190;
            Hue = 0x8FD;

            SetStr(305, 425);
            SetDex(72, 150);
            SetInt(505, 750);

            SetHits(12000);
            SetMana(2500);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Cold, 75);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 90, 100);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.Fishing, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500;
            Karma = 22500;

            VirtualArmor = 70;
			
            AddItem(new FancyShirt(0x84));
            AddItem(new LongPants(0x455));
            AddItem(new Boots(0x455));

            HairItemID = 0x203C; // Long Hair
            HairHue = 0x455;
            FacialHairItemID = 0x204B; // Medium Short Beard
            FacialHairHue = 0x455;
        }

        public UltimateMasterFishing(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(OceanicHarpoon), typeof(DivingSuit) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(AquamarineNecklace), typeof(CoralRing) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(OceanInABottle), typeof(ShipInABottle) }; }
        }

        public override MonsterStatuetteType[] StatueTypes
        {
            get { return new MonsterStatuetteType[] { }; }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, 6);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new PowerScroll(SkillName.Fishing, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new OceanicHarpoon());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new DivingSuit());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: TidalWave(); break;
                    case 1: NetToss(defender); break;
                    case 2: MarineCall(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void TidalWave()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && m.Player && this.CanBeHarmful(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                DoHarmful(m);

                int damage = Utility.RandomMinMax(60, 80);

                AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);

                m.FixedParticles(0x36CB, 20, 10, 5044, EffectLayer.Head);
                m.PlaySound(0x5C3);
            }
        }

        public void NetToss(Mobile defender)
        {
            if (defender != null)
            {
                defender.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                defender.PlaySound(0x22E);

                defender.Paralyze(TimeSpan.FromSeconds(6.0));

                defender.SendLocalizedMessage(1070836); // You have been caught in a net!
            }
        }

        public void MarineCall()
        {
            Map map = this.Map;

            if (map == null)
                return;

            int newSeaCreatures = Utility.RandomMinMax(3, 6);

            for (int i = 0; i < newSeaCreatures; ++i)
            {
                BaseCreature seaCreature;

                switch (Utility.Random(3))
                {
                    case 0: seaCreature = new SeaSerpent(); break;
                    case 1: seaCreature = new DeepSeaSerpent(); break;
                    default: seaCreature = new WaterElemental(); break;
                }

                seaCreature.Team = this.Team;

                bool validLocation = false;
                Point3D loc = this.Location;

                for (int j = 0; !validLocation && j < 10; ++j)
                {
                    int x = X + Utility.Random(3) - 1;
                    int y = Y + Utility.Random(3) - 1;
                    int z = map.GetAverageZ(x, y);

                    if (validLocation = map.CanFit(x, y, this.Z, 16, false, false))
                        loc = new Point3D(x, y, Z);
                    else if (validLocation = map.CanFit(x, y, z, 16, false, false))
                        loc = new Point3D(x, y, z);
                }

                seaCreature.MoveToWorld(loc, map);
                seaCreature.Combatant = this.Combatant;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}