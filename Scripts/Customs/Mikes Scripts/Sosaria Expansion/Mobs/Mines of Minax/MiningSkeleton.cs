using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a mining skeleton corpse")]
    public class MiningSkeleton : BaseCreature
    {
        // Cooldown timers
        private DateTime m_NextQuakeTime;
        private DateTime m_NextShardTime;
        private DateTime m_NextDustTime;
        private Point3D m_LastLocation;

        // Charcoal‑gray hue for ore‑infused bones
        private const int UniqueHue = 1175;

        [Constructable]
        public MiningSkeleton()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a mining skeleton";
            Body = Utility.RandomList(50, 56);
            BaseSoundID = 0x48D;
            Hue = UniqueHue;

            // Stats
            SetStr(200, 250);
            SetDex(80, 120);
            SetInt(60, 80);

            SetHits(800, 1000);
            SetStam(150, 200);
            SetMana(50, 80);

            SetDamage(10, 15);

            // Damage types: mostly physical with a molten edge
            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Fire, 30);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 20, 30);

            // Skills
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);
            SetSkill(SkillName.MagicResist, 80.1, 95.0);
            SetSkill(SkillName.DetectHidden, 60.0, 80.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 60;
            ControlSlots = 3;

            // Start cooldowns
            m_NextQuakeTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextShardTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 14));
            m_NextDustTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 22));
            m_LastLocation   = this.Location;

            // Loot: gritty ore
            PackItem(new IronOre(Utility.RandomMinMax(20, 30)));
            if (Utility.RandomDouble() < 0.05)
                PackItem(new SunkenThunder(1));
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Tremor Stomp (close-range AoE)
            if (DateTime.UtcNow >= m_NextQuakeTime && InRange(Combatant.Location, 3))
            {
                TremorStomp();
                m_NextQuakeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
                return;
            }

            // Ore Shard Volley (ranged)
            if (DateTime.UtcNow >= m_NextShardTime && InRange(Combatant.Location, 12))
            {
                OreShardVolley();
                m_NextShardTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
                return;
            }

            // Toxic Dust Cloud (area hazard)
            if (DateTime.UtcNow >= m_NextDustTime)
            {
                ToxicDustCloud();
                m_NextDustTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // DROPS landmines behind as it moves
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (this.Location != m_LastLocation && Alive && Map != null && Map != Map.Internal && Utility.RandomDouble() < 0.2)
            {
                Point3D dropLoc = m_LastLocation;
                if (!Map.CanFit(dropLoc.X, dropLoc.Y, dropLoc.Z, 16, false, false))
                    dropLoc.Z = Map.GetAverageZ(dropLoc.X, dropLoc.Y);

                var mine = new LandmineTile();
                mine.Hue = UniqueHue;
                mine.MoveToWorld(dropLoc, Map);
            }
            m_LastLocation = this.Location;
        }

        // Close‑range quake
        private void TremorStomp()
        {
            PlaySound(0x1F7); // Heavy impact
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3779, 8, 20, UniqueHue, 0, 5032, 0); // Dust ring

            var list = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 3))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    list.Add(m);
            }

            foreach (var m in list)
            {
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(40, 60), 100, 0, 0, 0, 0);
            }
        }

        // Medium‑range ore shards
		private void OreShardVolley()
		{
			if (Combatant is Mobile target && CanBeHarmful(target, false) && SpellHelper.ValidIndirectTarget(this, target))
			{
				Say("*Rattle… CLANG!*");
				PlaySound(0x2D1); // Shard launch

				// Visual send
				Effects.SendMovingParticles(
					new Entity(Serial.Zero, this.Location, this.Map),
					new Entity(Serial.Zero, target.Location, target.Map),
					0x36A4, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Waist, 0);

				Timer.DelayCall(TimeSpan.FromSeconds(0.3), () =>
				{
					if (CanBeHarmful(target, false))
					{
						DoHarmful(target);
						int dmg = Utility.RandomMinMax(25, 35);
						AOS.Damage(target, this, dmg, 100, 0, 0, 0, 0);

						// Sap stamina
						int sap = Utility.RandomMinMax(10, 20);
						if (target.Stam >= sap)
							target.Stam -= sap;
						target.SendMessage(0x22, "You feel your strength shaken by flying debris!");
					}
				});
			}
		}


        // Area poison/toxic hazard
        private void ToxicDustCloud()
        {
            Say("*Crackle… hiss!*");
            PlaySound(0x3E9); // Gas hiss

            // Scatter ToxicGasTile around self
            for (int i = 0; i < 6; i++)
            {
                int x = X + Utility.RandomMinMax(-2, 2);
                int y = Y + Utility.RandomMinMax(-2, 2);
                int z = Z;
                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                var gas = new ToxicGasTile();
                gas.Hue = UniqueHue;
                gas.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        public override bool BleedImmune  => true;
        public override Poison PoisonImmune => Poison.Lesser;
        public override TribeType Tribe   => TribeType.Undead;
        public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;
        public override int TreasureMapLevel => 5;
        public override double DispelDifficulty => 130.0;
        public override double DispelFocus     => 60.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 10));
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls, 2);
        }

        public MiningSkeleton(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset timers
            m_NextQuakeTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextShardTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 14));
            m_NextDustTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 22));
            m_LastLocation   = this.Location;
        }
    }
}
