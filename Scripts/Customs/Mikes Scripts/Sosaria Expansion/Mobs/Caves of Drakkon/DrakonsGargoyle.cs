using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;    // For SpellHelper

namespace Server.Mobiles
{
    [CorpseName("a drakons gargoyle corpse")]
    public class DrakonsGargoyle : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime _nextShadowBreath;
        private DateTime _nextVenomNova;
        private DateTime _nextSpikeSurge;
        private DateTime _nextWingGust;

        // Unique hue for the Drakons Gargoyle (Dark Slate Gray)
        private const int UniqueHue = 1154;

        [Constructable]
        public DrakonsGargoyle()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name           = "Drakons Gargoyle";
            Body           = 0x2F3;      // Gargoyle body
            BaseSoundID    = 0x174;      // Gargoyle sounds
            Hue            = UniqueHue;

            // Stats
            SetStr( 900, 1000 );
            SetDex( 180, 250 );
            SetInt( 300, 350 );

            SetHits( 650, 700 );
            SetDamage( 25, 35 );

            // Resistances
            SetResistance( ResistanceType.Physical, 50, 70 );
            SetResistance( ResistanceType.Fire,     60, 80 );
            SetResistance( ResistanceType.Cold,     40, 50 );
            SetResistance( ResistanceType.Poison,   30, 40 );
            SetResistance( ResistanceType.Energy,   40, 50 );

            // Skills
            SetSkill( SkillName.Wrestling,    100.0, 120.0 );
            SetSkill( SkillName.Tactics,      100.0, 120.0 );
            SetSkill( SkillName.MagicResist,  120.0, 150.0 );
            SetSkill( SkillName.Magery,        80.0, 100.0 );
            SetSkill( SkillName.EvalInt,       80.0, 100.0 );
            SetSkill( SkillName.Anatomy,       80.0, 100.0 );
            SetSkill( SkillName.Macing,        90.0, 110.0 );

            Fame       = 15000;
            Karma      = -15000;
            VirtualArmor = 70;

            // Initialize cooldowns
            _nextShadowBreath = DateTime.UtcNow;
            _nextVenomNova    = DateTime.UtcNow;
            _nextSpikeSurge   = DateTime.UtcNow;
            _nextWingGust     = DateTime.UtcNow;
        }

        public DrakonsGargoyle( Serial serial )
            : base( serial )
        {
        }

        public override bool CanFly        => true;
        public override bool BardImmune    => !Core.AOS;
        public override int  Meat          => 1;
        public override HideType HideType  => HideType.Barbed;
        public override int  Hides         => 25;
        public override int  Scales        => 10;
        public override ScaleType ScaleType => (ScaleType)Utility.Random(4);

        public override void GenerateLoot()
        {
            AddLoot( LootPack.Rich );
            AddLoot( LootPack.Gems, 5 );

            if ( Utility.RandomDouble() < 0.005 )     // 0.5% chance
                PackItem( new GargoylesPickaxe() );  // special shard pick

            if ( Utility.RandomDouble() < 0.02 )      // 2% chance
                PackItem( new TreasureMap( 6, Map ) );
        }

        // --- Special Abilities ---

        // 1) Shadow Breath: a spreading cone of necrotic energy + spawn NecromanticFlamestrikeTile
        public void ShadowBreathCone()
        {
			if (!(Combatant is Mobile)) return;
			Mobile target = (Mobile)Combatant;

            const int range = 8;
            const int width = 3;
            const int damage = 30;

            Map map = Map;
            if ( map == null || !Utility.InRange( Location, target.Location, range ) )
                return;

            // Direction vector
            Direction d = GetDirectionTo( target );
            int dx = 0, dy = 0;
            Movement.Movement.Offset( d, ref dx, ref dy );

            Effects.PlaySound( Location, map, 0x5F5 ); // spooky breath
            Effects.SendLocationParticles( EffectItem.Create( Location, map, TimeSpan.FromSeconds( 0.5 ) ),
                                           0x3709, 10, 30, UniqueHue, 0, 5052, 0 );

            var conePoints = new List<Point3D>();
            for ( int i = 1; i <= range; i++ )
            {
                int spread = (int)(i * (width / (double)range));
                for ( int s = -spread; s <= spread; s++ )
                {
                    int x = X + i * dx + (dy == 0 ? s : 0);
                    int y = Y + i * dy + (dx == 0 ? s : 0);
                    var p = new Point3D( x, y, Z );

                    if ( map.CanFit( p, 16, false, false ) )
                        conePoints.Add( p );
                    else
                    {
                        int z2 = map.GetAverageZ( x, y );
                        var p2 = new Point3D( x, y, z2 );
                        if ( map.CanFit( p2, 16, false, false ) )
                            conePoints.Add( p2 );
                    }
                }
            }

            foreach ( var p in conePoints )
            {
                Effects.SendLocationParticles( EffectItem.Create( p, map, TimeSpan.FromSeconds( 0.3 ) ),
                                               0x373A, 8, 8, UniqueHue, 0, 2036, 0 );
                AOS.Damage( target, this, damage, 0, 0, 100, 0, 0 );
                // spawn lingering necrotic flame tile
                // var tile = new NecromanticFlamestrikeTile();
                // tile.MoveToWorld( p, map );
            }

            _nextShadowBreath = DateTime.UtcNow + TimeSpan.FromSeconds( 12 );
        }

        // 2) Venom Nova: radial poison blast + spawn PoisonTile
        public void VenomNova()
        {
            const int radius = 6;
            const int damage = 20;

            Map map = Map;
            if ( map == null ) return;

            Effects.PlaySound( Location, map, 0x22F );
            Effects.SendLocationParticles( EffectItem.Create( Location, map, TimeSpan.FromSeconds( 0.5 ) ),
                                           0x36AB, 16, 30, UniqueHue, 0, 2041, 0 );

            for ( int dx = -radius; dx <= radius; dx++ )
            for ( int dy = -radius; dy <= radius; dy++ )
            {
                var p = new Point3D( X + dx, Y + dy, Z );
                if ( Utility.InRange( Location, p, radius ) && map.CanFit( p, 16, false, false ) )
                {
                    Effects.SendLocationParticles( EffectItem.Create( p, map, TimeSpan.FromSeconds( 0.3 ) ),
                                                   0x37C4, 6, 6, UniqueHue, 0, 2041, 0 );
                    foreach ( var m in map.GetMobilesInRange( p, 0 ) )
                    {
                        if ( m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)) )
                        {
                            AOS.Damage( m, this, damage, 0, 0, 100, 0, 0 );  // poison
                            // m.ApplyPoison( this, Poison.Deadly );
                        }
                    }
                    map.GetMobilesInRange( p, 0 ).Free();
                    // var tile = new PoisonTile();
                    // tile.MoveToWorld( p, map );
                }
            }

            _nextVenomNova = DateTime.UtcNow + TimeSpan.FromSeconds( 18 );
        }

        // 3) Stone Spike Surge: directional line of impaling spikes + spawn IceShardTile
        public void SpikeSurge()
        {
			if (!(Combatant is Mobile)) return;
			Mobile target = (Mobile)Combatant;

            const int length = 10;
            const int damage = 40;

            Map map = Map;
            if ( map == null ) return;

            Direction d = GetDirectionTo( target );
            int dx = 0, dy = 0;
            Movement.Movement.Offset( d, ref dx, ref dy );

            Effects.PlaySound( Location, map, 0x11D );
            Effects.SendLocationParticles( EffectItem.Create( Location, map, TimeSpan.FromSeconds( 0.5 ) ),
                                           0x36BD, 12, 12, UniqueHue, 0, 2072, 0 );

            for ( int i = 1; i <= length; i++ )
            {
                var p = new Point3D( X + dx * i, Y + dy * i, Z );
                if ( !map.CanFit( p, 16, false, false ) ) break;

                Effects.SendLocationParticles( EffectItem.Create( p, map, TimeSpan.FromSeconds( 0.25 ) ),
                                               0x3779, 8, 8, UniqueHue, 0, 2072, 0 );
                foreach ( var m in map.GetMobilesInRange( p, 0 ) )
                {
                    if ( m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)) )
                    {
                        AOS.Damage( m, this, damage, 100, 0, 0, 0, 0 );
                    }
                }
                map.GetMobilesInRange( p, 0 ).Free();
                // var tile = new IceShardTile();
                // tile.MoveToWorld( p, map );
            }

            _nextSpikeSurge = DateTime.UtcNow + TimeSpan.FromSeconds( 10 );
        }

        // 4) Wing Gust: triangular blast pushing foes back + spawn EarthquakeTile
        public void WingGust()
        {
			if (!(Combatant is Mobile)) return;
			Mobile target = (Mobile)Combatant;

            const int range = 6;
            const int damage = 25;

            Map map = Map;
            if ( map == null ) return;

            Effects.PlaySound( Location, map, 0x64B );
            Effects.SendLocationParticles( EffectItem.Create( Location, map, TimeSpan.FromSeconds( 0.5 ) ),
                                           0x3739, 10, 20, UniqueHue, 0, 2026, 0 );

            var points = new List<Point3D>();
            for ( int i = 1; i <= range; i++ )
            {
                for ( int j = -i; j <= i; j++ )
                {
                    int x = X + GetDirectionOffset( target, true ).dx * i + GetPerpOffset( target, j ).dx;
                    int y = Y + GetDirectionOffset( target, true ).dy * i + GetPerpOffset( target, j ).dy;
                    var p = new Point3D( x, y, Z );
                    if ( map.CanFit( p, 16, false, false ) )
                        points.Add( p );
                }
            }

            foreach ( var p in points )
            {
                Effects.SendLocationParticles( EffectItem.Create( p, map, TimeSpan.FromSeconds( 0.25 ) ),
                                               0x3709, 6, 6, UniqueHue, 0, 2026, 0 );
                foreach ( var m in map.GetMobilesInRange( p, 0 ) )
                {
                    if ( m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)) )
                    {
                        AOS.Damage( m, this, damage, 0, 0, 0, 0, 100 );
                        // optionally push back:
                        // SpellHelper.PushBack( this, m, 1 );
                    }
                }
                map.GetMobilesInRange( p, 0 ).Free();
                // var tile = new EarthquakeTile();
                // tile.MoveToWorld( p, map );
            }

            _nextWingGust = DateTime.UtcNow + TimeSpan.FromSeconds( 16 );
        }

        // --- AI Logic ---
        public override void OnThink()
        {
            base.OnThink();

            if ( Combatant is Mobile target )
            {
                if ( DateTime.UtcNow >= _nextShadowBreath && InRange( target, 8 ) )
                    ShadowBreathCone();
                else if ( DateTime.UtcNow >= _nextVenomNova )
                    VenomNova();
                else if ( DateTime.UtcNow >= _nextSpikeSurge && InRange( target, 10 ) )
                    SpikeSurge();
                else if ( DateTime.UtcNow >= _nextWingGust )
                    WingGust();
            }
        }

        public override void OnDamagedBySpell( Mobile from )
        {
            base.OnDamagedBySpell( from );
            if ( from != null && from.Alive && Utility.RandomDouble() < 0.3 )
                SpikeSurge();
        }

        public override void OnGotMeleeAttack( Mobile attacker )
        {
            base.OnGotMeleeAttack( attacker );
            if ( attacker != null && attacker.Alive && attacker.Weapon is BaseRanged && Utility.RandomDouble() < 0.3 )
                SpikeSurge();
        }

        // --- Death Explosion ---
        public override void OnDeath( Container c )
        {
            Map map = Map;
            if ( map != null )
            {
                Effects.PlaySound( Location, map, 0x208 );
                Effects.SendLocationParticles( EffectItem.Create( Location, map, TimeSpan.FromSeconds( 0.5 ) ),
                                               0x3709, 20, 60, UniqueHue, 0, 2041, 0 );

                for ( int i = 0; i < 15; i++ )
                {
                    int rx = Utility.RandomMinMax( -5, 5 );
                    int ry = Utility.RandomMinMax( -5, 5 );
                    var p = new Point3D( X + rx, Y + ry, Z );
                    if ( map.CanFit( p, 16, false, false ) )
                    {
                        // var web = new TrapWeb();
                        // web.MoveToWorld( p, map );
                        // var vortex = new VortexTile();
                        // vortex.MoveToWorld( p, map );
                        Effects.SendLocationParticles( EffectItem.Create( p, map, TimeSpan.FromSeconds( 0.3 ) ),
                                                       0x373A, 8, 8, UniqueHue, 0, 2026, 0 );
                    }
                }
            }

            base.OnDeath( c );
        }

        // --- Serialization ---
        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int)0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();
        }

        // Helpers for WingGust offsets
		private static (int dx, int dy) GetDirectionOffset(Mobile from, bool toward)
		{
			Direction d = from == null
				? Direction.North
				: from.Direction; // <- fixed here

			int dx = 0, dy = 0;
			Movement.Movement.Offset(d, ref dx, ref dy);
			return toward ? (dx, dy) : (-dx, -dy);
		}

		private static (int dx, int dy) GetPerpOffset(Mobile from, int amount)
		{
			Direction d = from == null
				? Direction.East
				: from.Direction; // <- fixed here

			int dx = 0, dy = 0;
			Movement.Movement.Offset((Direction)(((int)d + 2) % 8), ref dx, ref dy);
			return (dx * amount, dy * amount);
		}

    }
}
