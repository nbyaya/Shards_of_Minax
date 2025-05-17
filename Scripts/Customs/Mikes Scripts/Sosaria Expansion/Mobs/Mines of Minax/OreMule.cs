using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an ore mule corpse")]
    public class OreMule : BaseCreature
    {
        // Cooldown timers
        private DateTime m_NextShardTime;
        private DateTime m_NextQuakeTime;
        private DateTime m_NextMagnetTime;

        // Unique metallic hue
        private const int UniqueHue = 2635;

        [Constructable]
        public OreMule()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an ore mule";
            Body = 291;            // PackHorse body
            BaseSoundID = 0xA8;    // PackHorse sounds
            Hue = UniqueHue;

            // --- Stats ---
            SetStr(200, 250);
            SetDex(50, 70);
            SetInt(10, 20);

            SetHits(150, 180);
            SetStam(200, 250);
            SetMana(0);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire,     30, 40);
            SetResistance(ResistanceType.Cold,     40, 50);
            SetResistance(ResistanceType.Poison,   20, 30);
            SetResistance(ResistanceType.Energy,   20, 30);

            SetSkill(SkillName.Tactics,      100.0, 110.0);
            SetSkill(SkillName.Wrestling,    100.0, 110.0);
            SetSkill(SkillName.MagicResist,   80.0,  90.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 60;
            ControlSlots = 3;

            // Initialize ability cooldowns
            m_NextShardTime  = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax( 8, 12 ) );
            m_NextQuakeTime  = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax( 25, 35 ) );
            m_NextMagnetTime = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax( 15, 25 ) );
        }

        public override void OnThink()
        {
            base.OnThink();

            if ( Combatant == null || Map == null || Map == Map.Internal || !Alive )
                return;

            // Shard Barrage (ranged AoE)
            if ( DateTime.UtcNow >= m_NextShardTime && InRange( Combatant.Location, 12 ) )
            {
                ShardBarrage();
                m_NextShardTime = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax( 12, 18 ) );
            }

            // Earthquake Stamp (close‐range AoE tile + damage)
            if ( DateTime.UtcNow >= m_NextQuakeTime && InRange( Combatant.Location, 2 ) )
            {
                EarthquakeStamp();
                m_NextQuakeTime = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax( 25, 35 ) );
            }

            // Magnetic Pulse (deploys MagnetTile on target)
            if ( DateTime.UtcNow >= m_NextMagnetTime && InRange( Combatant.Location, 10 ) )
            {
                MagneticPulse();
                m_NextMagnetTime = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax( 20, 30 ) );
            }
        }

        private void ShardBarrage()
        {
            if ( Combatant is Mobile target && CanBeHarmful( target, false ) )
            {
                Say( "*The ore mule rattles its iron-laden flanks!*" );
                PlaySound( 0x29B ); // metallic rattle

                var hits = new List<Mobile>();
                foreach ( Mobile m in Map.GetMobilesInRange( Location, 12 ) )
                {
                    if ( m != this && CanBeHarmful( m, false ) && InLOS( m ) )
                        hits.Add( m );
                }

                foreach ( var m in hits )
                {
                    DoHarmful( m );
                    int dmg = Utility.RandomMinMax( 25, 40 );
                    AOS.Damage( m, this, dmg, 100, 0, 0, 0, 0 ); // 100% physical
                    m.FixedParticles( 0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head );
                }
            }
        }

        private void EarthquakeStamp()
        {
            Say( "*Stampedes the ground!*" );
            PlaySound( 0x20E ); // heavy stomp

            // Create EarthquakeTile in 5×5 around self
            for ( int x = X - 2; x <= X + 2; x++ )
            for ( int y = Y - 2; y <= Y + 2; y++ )
            {
                var loc = new Point3D( x, y, Z );
                int z = loc.Z;
                if ( !Map.CanFit( x, y, z, 16, false, false ) )
                    z = Map.GetAverageZ( x, y );

                if ( Map.CanFit( x, y, z, 16, false, false ) )
                {
                    var tile = new EarthquakeTile { Hue = UniqueHue };
                    tile.MoveToWorld( new Point3D( x, y, z ), Map );
                }
            }

            // Damage everybody very close
            foreach ( Mobile m in Map.GetMobilesInRange( Location, 2 ) )
            {
                if ( m != this && CanBeHarmful( m, false ) )
                {
                    DoHarmful( m );
                    int dmg = Utility.RandomMinMax( 30, 50 );
                    AOS.Damage(m, this, dmg, false, 0, 0, 0, 0, 0, 0, 100, false, false, false);
                }
            }
        }

        private void MagneticPulse()
        {
            if ( Combatant is Mobile target && CanBeHarmful( target, false ) )
            {
                Say( "*Rumbles with magnetic force!*" );
                PlaySound( 0x5A ); // low rumble

                // Drop a MagnetTile under the target
                var tile = new MagnetTile { Hue = UniqueHue };
                tile.MoveToWorld( target.Location, Map );
            }
        }

		public override void OnDeath(Container c)
		{
			

			Say("*The ore mule collapses, scattering ore fragments!*");

			if (c != null)
			{
				// Scatter random ore in its corpse
				for (int i = 0; i < Utility.RandomMinMax(5, 8); i++)
				{
					Item ore = Utility.RandomBool()
						? (Item)new IronOre()
						: Utility.RandomBool()
							? new GoldOre()
							: new GoldOre();

					c.DropItem(ore);
				}
			}

			// Lay a few LandmineTiles around
			for (int i = 0; i < 4; i++)
			{
				int dx = Utility.RandomMinMax(-3, 3);
				int dy = Utility.RandomMinMax(-3, 3);
				var loc = new Point3D(X + dx, Y + dy, Z);
				int z = loc.Z;

				if (!Map.CanFit(loc.X, loc.Y, z, 16, false, false))
					z = Map.GetAverageZ(loc.X, loc.Y);

				if (Map.CanFit(loc.X, loc.Y, z, 16, false, false))
				{
					var mine = new LandmineTile { Hue = UniqueHue };
					mine.MoveToWorld(new Point3D(loc.X, loc.Y, z), Map);
				}
			}
			
			base.OnDeath(c);
		}


        public override void GenerateLoot()
        {
            AddLoot( LootPack.UltraRich );
            if ( Utility.RandomDouble() < 0.05 ) // 5% chance
                PackItem( new FangOfTheFifthEye() ); // your custom artifact
        }

        public OreMule( Serial serial )
            : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int)0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();

            // Re‐initialize cooldowns
            m_NextShardTime  = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax( 8, 12 ) );
            m_NextQuakeTime  = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax( 25, 35 ) );
            m_NextMagnetTime = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax( 15, 25 ) );
        }
    }
}
