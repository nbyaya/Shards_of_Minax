using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a shadow wolf corpse")]
    public class ShadowWolf : BaseCreature
    {
        // Cooldown timers
        private DateTime m_NextLeapTime;
        private DateTime m_NextHowlTime;
        private DateTime m_NextShadowStepTime;
        private Point3D m_LastLocation;

        // Unique hue for the Shadow Wolf (deep, inky black-purple)
        private const int UniqueHue = 1175;

        [Constructable]
        public ShadowWolf()
            : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4 )
        {
            Name           = "a shadow wolf";
            Body           = 719;        // same body as DragonWolf
            BaseSoundID    = 0x5ED;
            Hue            = UniqueHue;

            // Enhanced Base Stats
            SetStr( 900, 1000 );
            SetDex( 120, 140 );
            SetInt( 300, 350 );

            SetHits( 1500, 1800 );
            SetStam( 300, 350 );
            SetMana( 200, 250 );

            SetDamage( 25, 35 );

            // Damage types
            SetDamageType( ResistanceType.Physical, 60 );
            SetDamageType( ResistanceType.Cold,     40 );

            // Resistances
            SetResistance( ResistanceType.Physical, 50, 60 );
            SetResistance( ResistanceType.Fire,     30, 40 );
            SetResistance( ResistanceType.Cold,     60, 70 );
            SetResistance( ResistanceType.Poison,   50, 60 );
            SetResistance( ResistanceType.Energy,   50, 60 );

            // Skills
            SetSkill( SkillName.Tactics,        100.0, 120.0 );
            SetSkill( SkillName.Wrestling,      100.0, 120.0 );
            SetSkill( SkillName.MagicResist,    100.0, 120.0 );
            SetSkill( SkillName.DetectHidden,    80.0, 100.0 );
            SetSkill( SkillName.Poisoning,       80.0, 100.0 );

            Fame           = 25000;
            Karma         = -25000;

            VirtualArmor  = 90;
            ControlSlots   = 6;

            // Schedule first uses of each special ability
            m_NextLeapTime        = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax(10, 15) );
            m_NextHowlTime        = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax(20, 30) );
            m_NextShadowStepTime  = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax(15, 25) );

            m_LastLocation = this.Location;

            // Base loot
            PackItem( new BatWing(  Utility.RandomMinMax(5, 10) ) );
        }

        public ShadowWolf( Serial serial ) : base( serial )
        {
        }

        // Passive aura: drains stamina and inflicts poison on anyone who comes too close
        public override void OnMovement( Mobile m, Point3D oldLocation )
        {
            if ( m == this || !Alive || m.Map != this.Map )
            {
                base.OnMovement( m, oldLocation );
                return;
            }

            if ( m.InRange( this.Location, 2 ) && CanBeHarmful( m, false ) && m is Mobile target )
            {
                DoHarmful( target );

                // Drain stamina
                int stamDrain = Utility.RandomMinMax( 10, 20 );
                if ( target.Stam >= stamDrain )
                {
                    target.Stam -= stamDrain;
                    target.SendMessage( 0x26, "An icy chill seizes your legs!" );
                    target.FixedParticles( 0x374A, 8, 12, 5032, EffectLayer.Head );
                }

                // Apply minor poison
                target.ApplyPoison( this, Poison.Deadly );
            }

            base.OnMovement( m, oldLocation );
        }

        public override void OnThink()
        {
            base.OnThink();

            if ( Combatant == null || !Alive || Map == null || Map == Map.Internal )
                return;

            var now = DateTime.UtcNow;

            // Leap Attack: jump onto combatant and drop necrotic flames at landing
            if ( now >= m_NextLeapTime && this.InRange( Combatant.Location, 8 ) )
            {
                LeapAttack();
                m_NextLeapTime = now + TimeSpan.FromSeconds( Utility.RandomMinMax(15, 22) );
            }
            // Umbral Howl: fear and immobilize around self
            else if ( now >= m_NextHowlTime && this.InRange( Combatant.Location, 12 ) )
            {
                UmbralHowl();
                m_NextHowlTime = now + TimeSpan.FromSeconds( Utility.RandomMinMax(25, 35) );
            }
            // Shadow Step: teleport behind the target and bite
            else if ( now >= m_NextShadowStepTime && this.InRange( Combatant.Location, 12 ) )
            {
                ShadowStep();
                m_NextShadowStepTime = now + TimeSpan.FromSeconds( Utility.RandomMinMax(18, 26) );
            }

            // Track last location
            m_LastLocation = this.Location;
        }

        private void LeapAttack()
        {
            if ( Combatant is Mobile target && CanBeHarmful( target, false ) )
            {
                this.Say( "*The shadows propel me!*" );
                this.PlaySound( 0x2F3 );



                // Damage the target
                DoHarmful( target );
                int dmg = Utility.RandomMinMax( 60, 80 );
                AOS.Damage( target, this, dmg, 100, 0, 0, 0, 0 );

                // Place a necrotic flame tile at the landing spot
                var loc = target.Location;
                Timer.DelayCall( TimeSpan.FromSeconds( 0.3 ), () =>
                {
                    if ( this.Map != null && Map.CanFit( loc.X, loc.Y, loc.Z, 16, false, false ) )
                    {
                        var tile = new NecromanticFlamestrikeTile();
                        tile.Hue = UniqueHue;
                        tile.MoveToWorld( loc, this.Map );
                    }
                } );
            }
        }

        private void UmbralHowl()
        {
            this.Say( "*Woooooooowl of night!*" );
            this.PlaySound( 0x2C4 );

            // Affect all mobiles in radius 6
            List<Mobile> list = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange( this.Location, 6 );
            foreach ( Mobile m in eable )
            {
                if ( m != this && m is Mobile target && CanBeHarmful( target, false ) )
                {
                    list.Add( target );
                }
            }
            eable.Free();

            foreach ( Mobile t in list )
            {
                DoHarmful( t );
                // Fear effect
                t.Freeze( TimeSpan.FromSeconds( 3.0 ) );
                t.SendMessage( 0x22, "The wolf's howl freezes you in terror!" );

                // TrapWeb tile under each
                var loc = t.Location;
                if ( Map.CanFit( loc.X, loc.Y, loc.Z, 16, false, false ) )
                {
                    var web = new TrapWeb();
                    web.MoveToWorld( loc, this.Map );
                }

                // Poison damage
                t.ApplyPoison( this, Poison.Lethal );
            }
        }

        private void ShadowStep()
        {
            if ( Combatant is Mobile target && CanBeHarmful( target, false ) )
            {
                this.Say( "*From the darkness, I strike!*" );
                this.PlaySound( 0x3EE );

                // Teleport to a point behind the target
                var dest = target.Location;
                Point3D step;
                if ( target.Direction == Direction.North )      step = new Point3D( dest.X, dest.Y + 1, dest.Z );
                else if ( target.Direction == Direction.South ) step = new Point3D( dest.X, dest.Y - 1, dest.Z );
                else if ( target.Direction == Direction.East )  step = new Point3D( dest.X - 1, dest.Y, dest.Z );
                else                                            step = new Point3D( dest.X + 1, dest.Y, dest.Z );

                if ( Map.CanFit( step.X, step.Y, step.Z, 16, false, false ) )
                    this.Location = step;

                // Bite attack with life leech
                DoHarmful( target );
                int dmg     = Utility.RandomMinMax( 40, 60 );
                int leech   = dmg / 2;
                AOS.Damage( target, this, dmg, 100, 0, 0, 0, 0 );
                this.Hits += leech; 
                this.PlaySound( 0x3F5 );
                target.SendMessage( 0x22, "Your life is siphoned into the darkness!" );
            }
        }

        public override void OnDeath( Container c )
        {
            if ( Map != null )
            {
                this.Say( "*Return to shadow...*" );
                Effects.PlaySound( this.Location, this.Map, 0x212 );
                Effects.SendLocationParticles( EffectItem.Create( this.Location, this.Map, EffectItem.DefaultDuration ), 0x3709, 10, 60, UniqueHue, 0, 5052, 0 );

                // Spawn quicksand traps around the corpse
                for ( int i = 0; i < 5; i++ )
                {
                    int dx = Utility.RandomMinMax( -3, 3 );
                    int dy = Utility.RandomMinMax( -3, 3 );
                    var loc = new Point3D( this.X + dx, this.Y + dy, this.Z );
                    if ( Map.CanFit( loc.X, loc.Y, loc.Z, 16, false, false ) )
                    {
                        var qs = new QuicksandTile();
                        qs.MoveToWorld( loc, this.Map );
                    }
                }
            }

            base.OnDeath( c );
        }

        public override void GenerateLoot()
        {
            AddLoot( LootPack.UltraRich, 2 );
            AddLoot( LootPack.Gems,        Utility.RandomMinMax( 10, 15 ) );
            AddLoot( LootPack.MedScrolls,  Utility.RandomMinMax( 2, 4 ) );

            // 3% chance to drop a unique "Shadow Essence"
            if ( Utility.RandomDouble() < 0.03 )
                PackItem( new Masksplitter() );
        }

        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }

        public override int TreasureMapLevel { get { return 6; } }
        public override double DispelDifficulty { get { return 150.0; } }
        public override double DispelFocus     { get { return 75.0;  } }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int)0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();

            // Reset ability timers after load
            m_NextLeapTime       = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax(10, 15) );
            m_NextHowlTime       = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax(20, 30) );
            m_NextShadowStepTime = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax(15, 25) );
        }
    }
}
