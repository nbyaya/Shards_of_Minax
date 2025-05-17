using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells; // For SpellHelper if needed

namespace Server.Mobiles
{
    [CorpseName("a drakon beetle corpse")]
    public class DrakonBeetle : BaseCreature
    {
        // Cooldown timers
        private DateTime m_NextAcidicBurst;
        private DateTime m_NextShellShock;
        private DateTime m_NextVenomWeb;
        private DateTime m_NextCarapaceOverload;

        // Unique emerald‐green hue
        private const int UniqueHue = 1257;

        [Constructable]
        public DrakonBeetle()
            : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
        {
            Name = "Drakon Beetle";
            Body = 0x317;
            BaseSoundID = 0x21D;
            Hue = UniqueHue;

            // Stats
            SetStr( 500, 600 );
            SetDex( 150, 200 );
            SetInt( 300, 400 );

            SetHits( 1200, 1400 );
            SetDamage( 20, 30 );

            // Resistances
            SetResistance( ResistanceType.Physical, 50, 60 );
            SetResistance( ResistanceType.Fire,     30, 40 );
            SetResistance( ResistanceType.Cold,     40, 50 );
            SetResistance( ResistanceType.Poison,   80, 90 ); // Highly poison‑resistant
            SetResistance( ResistanceType.Energy,   30, 40 );

            // Skills
            SetSkill( SkillName.MagicResist, 100.0, 120.0 );
            SetSkill( SkillName.Tactics,     120.0, 140.0 );
            SetSkill( SkillName.Wrestling,   120.0, 140.0 );

            Fame = 10000;
            Karma = -8000;
            VirtualArmor = 60;

            // Initialize cooldowns
            m_NextAcidicBurst     = DateTime.UtcNow;
            m_NextShellShock      = DateTime.UtcNow;
            m_NextVenomWeb        = DateTime.UtcNow;
            m_NextCarapaceOverload= DateTime.UtcNow;
        }

        public DrakonBeetle( Serial serial ) : base(serial) { }

        // Drops hazardous carapace shards on death
        public override void OnDeath( Container c )
        {
            Map map = this.Map;
            if ( map != null )
            {
                Effects.PlaySound( Location, map, 0x214 ); // heavy rumble
                Effects.SendLocationParticles(
                    EffectItem.Create( Location, map, EffectItem.DefaultDuration ),
                    0x3709, 20, 30, UniqueHue, 0, 5016, 0
                );

                // Scatter 15–20 poisonous tiles
                for( int i = 0; i < 18; i++ )
                {
                    Point3D p = GetRandomValidLocation( Location, 5, map );
                    if ( p != Point3D.Zero )
                    {
                        // Alternate between PoisonTile and LandmineTile
                        if ( Utility.RandomBool() )
                            new PoisonTile().MoveToWorld( p, map );
                        else
                            new LandmineTile().MoveToWorld( p, map );

                        Effects.SendLocationParticles(
                            EffectItem.Create( p, map, EffectItem.DefaultDuration ),
                            0x3709, 10, 15, UniqueHue, 0, 5035, 0
                        );
                    }
                }
            }

            base.OnDeath( c );
        }

        // --- AOE Abilities ---

        // 1) Acidic Burst: Circular poison cloud around self
        public void AcidicBurst()
        {
            Map map = this.Map;
            if ( map == null ) return;

            Effects.PlaySound( Location, map, 0x1F7 ); // fizzing acid
            Effects.SendLocationParticles(
                EffectItem.Create( Location, map, EffectItem.DefaultDuration ),
                0x36B0, 12, 20, UniqueHue, 0, 2023, 0
            );

            int radius = 6;
            int damage = 25;

            for( int x = -radius; x <= radius; x++ )
            {
                for( int y = -radius; y <= radius; y++ )
                {
                    Point3D p = new Point3D( X + x, Y + y, Z );
                    if ( map.CanFit( p, 16, false, false ) && Utility.InRange( Location, p, radius ) )
                    {
                        Effects.SendLocationParticles(
                            EffectItem.Create( p, map, EffectItem.DefaultDuration ),
                            0x37C1, 8, 10, UniqueHue, 0, 5052, 0
                        );

                        foreach( Mobile m in map.GetMobilesInRange( p, 0 ) )
                        {
                            if( m != this && (m is PlayerMobile || (m is BaseCreature && ((BaseCreature)m).Team != Team)) )
                            {
                                DoHarmful( m );
                                AOS.Damage( m, this, damage, 0, 0, 100, 0, 0 );
                            }
                        }
                    }
                }
            }

            m_NextAcidicBurst = DateTime.UtcNow + TimeSpan.FromSeconds(12);
        }

        // 2) Shell Shock: Long line blast ahead
        public void ShellShock()
        {
            Map map = this.Map;
            if ( map == null || Combatant == null ) return;
            if ( !(Combatant is Mobile target) ) return;

            Direction d = GetDirectionTo( target );
			int dx = 0, dy = 0;
			Movement.Movement.Offset(d, ref dx, ref dy);

            Effects.PlaySound( Location, map, 0x212 ); // heavy crash
            int length = 12, damage = 40;
            for( int i = 1; i <= length; i++ )
            {
                Point3D p = new Point3D( X + dx*i, Y + dy*i, Z );
                if ( !map.CanFit( p, 16, false, false ) ) break;

                Effects.SendLocationParticles(
                    EffectItem.Create( p, map, EffectItem.DefaultDuration ),
                    0x3728, 6, 6, UniqueHue, 0, 2023, 0
                );

                foreach( Mobile m in map.GetMobilesInRange( p, 0 ) )
                {
                    if( m != this && (m is PlayerMobile || (m is BaseCreature && ((BaseCreature)m).Team != Team)) )
                    {
                        DoHarmful( m );
                        AOS.Damage( m, this, damage, 100, 0, 0, 0, 0 );
                        // 30% chance to stun
                        if( Utility.RandomDouble() < 0.3 )
                            m.Paralyze( TimeSpan.FromSeconds( 2 ) );
                    }
                }
            }

            m_NextShellShock = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        // 3) Venomous Web: Cone of sticky webs
        public void VenomousWeb()
        {
            Map map = this.Map;
            if ( map == null || Combatant == null ) return;
            if ( !(Combatant is Mobile target) ) return;

            Effects.PlaySound( Location, map, 0x223 ); // web flick
            Effects.SendLocationParticles(
                EffectItem.Create( Location, map, EffectItem.DefaultDuration ),
                0x3709, 10, 30, UniqueHue, 0, 5052, 0
            );

            int range = 8, width = 3, damage = 15;
            Direction d = GetDirectionTo( target );
			int dx = 0, dy = 0;
			Movement.Movement.Offset(d, ref dx, ref dy);

            for( int i = 1; i <= range; i++ )
            {
                int spread = (int)(i * (width / (double)range));
                for( int j = -spread; j <= spread; j++ )
                {
                    int tx = X + dx*i + ((dy==0)?j:0);
                    int ty = Y + dy*i + ((dx==0)?j:0);
                    Point3D p = new Point3D( tx, ty, Z );
                    if ( map.CanFit( p, 16, false, false ) )
                    {
                        Effects.SendLocationParticles(
                            EffectItem.Create( p, map, EffectItem.DefaultDuration ),
                            0x36B0, 8, 8, UniqueHue, 0, 2023, 0
                        );

                        foreach( Mobile m in map.GetMobilesInRange( p, 0 ) )
                        {
                            if( m != this && (m is PlayerMobile || (m is BaseCreature && ((BaseCreature)m).Team != Team)) )
                            {
                                DoHarmful( m );
                                AOS.Damage( m, this, damage, 0, 0, 100, 0, 0 );
                                // slow for 3s
                                m.FixedParticles( 0x3779, 10, 20, 5042, EffectLayer.Waist );
                                m.Send( new AsciiMessage( m.Serial, m.Body, MessageType.Regular, 0x3B2, 3,
                                    "You are ensnared by thick webbing!", "" ) );
                                m.Paralyze( TimeSpan.FromSeconds(3) );
                            }
                        }

                        // spawn a TrapWeb tile 40% chance
                        if ( Utility.RandomDouble() < 0.4 )
                            new TrapWeb().MoveToWorld( p, map );
                    }
                }
            }

            m_NextVenomWeb = DateTime.UtcNow + TimeSpan.FromSeconds(14);
        }

        // 4) Carapace Overload: Electrified zone around self
        public void CarapaceOverload()
        {
            Map map = this.Map;
            if ( map == null ) return;

            Effects.PlaySound( Location, map, 0x1F1 ); // crackling energy
            Effects.SendLocationParticles(
                EffectItem.Create( Location, map, EffectItem.DefaultDuration ),
                0x37C4, 1, 10, UniqueHue, 0, 9909, 0
            );

            int radius = 4;
            for( int x = -radius; x <= radius; x++ )
            {
                for( int y = -radius; y <= radius; y++ )
                {
                    Point3D p = new Point3D( X + x, Y + y, Z );
                    if ( map.CanFit( p, 16, false, false ) && Utility.InRange( Location, p, radius ) )
                    {
                        Effects.SendLocationParticles(
                            EffectItem.Create( p, map, EffectItem.DefaultDuration ),
                            0x37CB, 1, 10, UniqueHue, 0, 9909, 0
                        );

                        foreach( Mobile m in map.GetMobilesInRange( p, 0 ) )
                        {
                            if( m != this && (m is PlayerMobile || (m is BaseCreature && ((BaseCreature)m).Team != Team)) )
                            {
                                DoHarmful( m );
                                AOS.Damage( m, this, 20, 0, 0, 0, 0, 100 );
                            }
                        }

                        // spawn a 20% chance LightningStormTile
                        if ( Utility.RandomDouble() < 0.2 )
                            new LightningStormTile().MoveToWorld( p, map );
                    }
                }
            }

            m_NextCarapaceOverload = DateTime.UtcNow + TimeSpan.FromSeconds(18);
        }

        // AI logic to pick abilities
        public override void OnThink()
        {
            base.OnThink();

            if ( Combatant is Mobile tgt )
            {
                if ( DateTime.UtcNow >= m_NextAcidicBurst )
                    AcidicBurst();
                else if ( DateTime.UtcNow >= m_NextShellShock && InRange( tgt, 12 ) )
                    ShellShock();
                else if ( DateTime.UtcNow >= m_NextVenomWeb && InRange( tgt, 8 ) )
                    VenomousWeb();
                else if ( DateTime.UtcNow >= m_NextCarapaceOverload )
                    CarapaceOverload();
            }
        }

        // Helpers
        private Point3D GetRandomValidLocation( Point3D center, int radius, Map map )
        {
            for ( int i = 0; i < 10; i++ )
            {
                int dx = Utility.RandomMinMax( -radius, radius );
                int dy = Utility.RandomMinMax( -radius, radius );
                Point3D p = new Point3D( center.X + dx, center.Y + dy, center.Z );

                if ( map.CanFit( p, 16, false, false ) && Utility.InRange( center, p, radius ) )
                    return p;

                int z = map.GetAverageZ( p.X, p.Y );
                Point3D p2 = new Point3D( p.X, p.Y, z );
                if ( map.CanFit( p2, 16, false, false ) && Utility.InRange( center, p2, radius ) )
                    return p2;
            }
            return Point3D.Zero;
        }

        // Sounds
        public override int GetIdleSound()   => 0x21D;
        public override int GetAngerSound()  => 0x21D;
        public override int GetAttackSound() => 0x162;
        public override int GetHurtSound()   => 0x163;
        public override int GetDeathSound()  => 0x21D;

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
    }
}
