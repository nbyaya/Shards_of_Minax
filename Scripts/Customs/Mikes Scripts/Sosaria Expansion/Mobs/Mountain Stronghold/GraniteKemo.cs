using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Spells;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a granite kemo corpse")]
    public class GraniteKemo : BaseCreature
    {
        // Cooldown timers
        private DateTime m_NextTremorTime;
        private DateTime m_NextShardBarrageTime;
        private DateTime m_NextQuakeWaveTime;
        private Point3D m_LastLocation;

        // Unique granite-gray hue
        private const int UniqueHue = 2001;

        [Constructable]
        public GraniteKemo()
            : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4 )
        {
            Name = "a granite kemo";
            Body = 196;
            BaseSoundID = 655;
            Hue = UniqueHue;

            // Stats
            SetStr( 450, 550 );
            SetDex( 150, 200 );
            SetInt( 100, 150 );

            SetHits( 1800, 2100 );
            SetStam( 200, 250 );
            SetMana( 200, 300 );

            // Damage
            SetDamage( 20, 30 );
            SetDamageType( ResistanceType.Physical, 80 );
            SetDamageType( ResistanceType.Cold, 20 );

            // Resistances
            SetResistance( ResistanceType.Physical, 70, 80 );
            SetResistance( ResistanceType.Fire, 40, 60 );
            SetResistance( ResistanceType.Cold, 50, 70 );
            SetResistance( ResistanceType.Poison, 60, 80 );
            SetResistance( ResistanceType.Energy, 30, 50 );

            // Skills
            SetSkill( SkillName.Tactics, 120.1, 130.0 );
            SetSkill( SkillName.Wrestling, 120.1, 130.0 );
            SetSkill( SkillName.MagicResist, 80.1, 95.0 );
            SetSkill( SkillName.Anatomy, 100.1, 110.0 );

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 100;
            ControlSlots = 5;

            // Initialize ability timers
            m_NextTremorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextShardBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextQuakeWaveTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));

            // Initial last location
            m_LastLocation = this.Location;

            // Basic loot
            PackItem( new Granite( Utility.RandomMinMax(10, 15) ) ); // shard resource
        }

        public GraniteKemo( Serial serial ) : base(serial) { }

        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 140.0;
        public override double DispelFocus => 70.0;

        public override void GenerateLoot()
        {
            AddLoot( LootPack.UltraRich, 2 );
            AddLoot( LootPack.Gems, Utility.RandomMinMax(8, 12) );
            AddLoot( LootPack.HighScrolls, Utility.RandomMinMax(1, 2) );

            if ( Utility.RandomDouble() < 0.02 ) // 2% chance at unique granite talisman
                PackItem( new MaxxiaScroll() );
        }

        public override void OnThink()
        {
            base.OnThink();

            if ( Combatant == null || Map == null || !Alive || Map == Map.Internal )
                return;

            var now = DateTime.UtcNow;

            // Tremor ability: AoE Earthquake beneath self
            if ( now >= m_NextTremorTime )
            {
                EarthquakeTremor();
                m_NextTremorTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
                return;
            }

            // Shard Barrage: chain of flying granite shards
            if ( now >= m_NextShardBarrageTime && InRange( Combatant.Location, 12 ) )
            {
                GraniteShardBarrage();
                m_NextShardBarrageTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
                return;
            }

            // Quake Wave: targeted ground hazard at combatant
            if ( now >= m_NextQuakeWaveTime && InRange( Combatant.Location, 12 ) )
            {
                QuakeWaveAttack();
                m_NextQuakeWaveTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 28));
                return;
            }
        }

        public override void OnMovement( Mobile m, Point3D oldLocation )
        {
            base.OnMovement(m, oldLocation);

            // Leave burrowing landmines occasionally as it moves
            if ( this.Location != m_LastLocation && Map != null && Map != Map.Internal && Utility.RandomDouble() < 0.20 )
            {
                var loc = m_LastLocation;
                m_LastLocation = this.Location;

                if ( Map.CanFit( loc.X, loc.Y, loc.Z, 16, false, false ) )
                {
                    var tile = new LandmineTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld( loc, this.Map );
                }
                else
                {
                    int z = Map.GetAverageZ( loc.X, loc.Y );
                    if ( Map.CanFit( loc.X, loc.Y, z, 16, false, false ) )
                    {
                        var tile = new LandmineTile();
                        tile.Hue = UniqueHue;
                        tile.MoveToWorld( new Point3D(loc.X, loc.Y, z), this.Map );
                    }
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }
        }

        // --- Ability: Earthquake Tremor ---
        private void EarthquakeTremor()
        {
            this.Say("*The ground trembles!*");
            this.PlaySound(0x2F3);
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x36BD, 20, 10, UniqueHue, 0, 5032, 0);

            // Damage all in 5-tile radius
            var list = new List<Mobile>();
            foreach ( var m in Map.GetMobilesInRange(this.Location, 5) )
            {
                if ( m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m) )
                    list.Add(m);
            }

            foreach ( var m in list )
            {
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(30, 50), 100, 0, 0, 0, 0);
            }

            // Leave earthquake tile hazard at feet
            var quake = new EarthquakeTile();
            quake.Hue = UniqueHue;
            quake.MoveToWorld(this.Location, this.Map);
        }

        // --- Ability: Granite Shard Barrage ---
        private void GraniteShardBarrage()
        {
            if ( !(Combatant is Mobile target) ) return;
            if ( !CanBeHarmful(target, false) || !SpellHelper.ValidIndirectTarget(this, target) ) return;

            this.Say("*Feel the stones strike!*");
            this.PlaySound(0x229);

            var hits = new List<Mobile> { target };
            int maxBounces = 4;

            for ( int i = 0; i < maxBounces; i++ )
            {
                Mobile last = hits[hits.Count - 1];
                Mobile next = null; double best = double.MaxValue;

                foreach ( var m in Map.GetMobilesInRange(last.Location, 8) )
                {
                    if ( m != this && m != last && !hits.Contains(m)
                        && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(last, m) && last.InLOS(m) )
                    {
                        double d = last.GetDistanceToSqrt(m);
                        if ( d < best ) { best = d; next = m; }
                    }
                }
                if ( next != null ) hits.Add(next);
                else break;
            }

            for ( int i = 0; i < hits.Count; i++ )
            {
                var src = (i == 0 ? this : hits[i - 1]);
                var dst = hits[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, src.Map),
                    new Entity(Serial.Zero, dst.Location, dst.Map),
                    0x36BE, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                var dmgTarget = dst;
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.1), () =>
                {
                    if ( CanBeHarmful(dmgTarget, false) )
                    {
                        DoHarmful(dmgTarget);
                        AOS.Damage(dmgTarget, this, Utility.RandomMinMax(25, 40), 100, 0, 0, 0, 0);
                        dmgTarget.FixedParticles(0x36CC, 1, 15, 9502, EffectLayer.Waist);
                    }
                });
            }
        }

        // --- Ability: Quake Wave ---
        private void QuakeWaveAttack()
        {
            if ( !(Combatant is Mobile target) ) return;
            if ( !CanBeHarmful(target, false) || !SpellHelper.ValidIndirectTarget(this, target) ) return;

            Point3D loc = target.Location;
            this.Say("*The earth splits beneath you!*");
            this.PlaySound(0x2F3);

            Effects.SendLocationParticles(
                EffectItem.Create(loc, this.Map, EffectItem.DefaultDuration),
                0x36BD, 20, 10, UniqueHue, 0, 5032, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if ( Map == null ) return;

                if ( !Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false) )
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var quake = new EarthquakeTile();
                quake.Hue = UniqueHue;
                quake.MoveToWorld(loc, this.Map);
            });
        }

        // --- Death Effect ---
		public override void OnDeath(Container c)
		{
			base.OnDeath(c);

			if (this.Map == null)
				return;

			this.Say("*Cracks... release the stones!*");
			Effects.PlaySound(this.Location, this.Map, 0x2F3);
			Effects.SendLocationParticles(
				EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
				0x36BD, 20, 10, UniqueHue, 0, 5032, 0);

			// Scatter landmine hazards
			int count = Utility.RandomMinMax(5, 8);
			for (int i = 0; i < count; i++)
			{
				int x = this.X + Utility.RandomMinMax(-4, 4);
				int y = this.Y + Utility.RandomMinMax(-4, 4);
				int z = this.Z;

				if (!Map.CanFit(x, y, z, 16, false, false))
					z = Map.GetAverageZ(x, y);

				if (Map.CanFit(x, y, z, 16, false, false))
				{
					var mine = new LandmineTile();
					mine.Hue = UniqueHue;
					mine.MoveToWorld(new Point3D(x, y, z), this.Map);
				}
			}
		}


        public override void Serialize( GenericWriter writer )
        {
            base.Serialize(writer);
            writer.Write( (int)0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset timers
            m_NextTremorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextShardBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextQuakeWaveTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_LastLocation = this.Location;
        }
    }
}
