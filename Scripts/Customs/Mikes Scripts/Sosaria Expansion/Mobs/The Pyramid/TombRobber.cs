using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.ContextMenus;

namespace Server.Mobiles
{
    [CorpseName("a tomb robber corpse")]
    public class TombRobber : BaseCreature
    {
        public override bool ClickTitle { get { return false; } }
        public override bool CanStealth  { get { return true;  } }

        private DateTime m_NextShadowStep;
        private DateTime m_NextTrap;
        private DateTime m_NextCurse;
        private DateTime m_NextSwipe;

        private const int UniqueHue = 1258; // Sand‑gold tint

        [Constructable]
        public TombRobber() : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4 )
        {
            Name        = "a tomb robber";
            SpeechHue   = Utility.RandomDyedHue();
            Hue         = UniqueHue;
            Body        = ( this.Female = Utility.RandomBool() ) ? 0x191 : 0x190;

            // —— Stats ——
            SetStr( 300, 350 );
            SetDex( 200, 250 );
            SetInt( 200, 250 );

            SetHits( 1500, 1800 );
            SetStam( 300, 350 );
            SetMana( 200, 250 );

            SetDamage( 20, 30 );

            SetDamageType( ResistanceType.Physical, 60 );
            SetDamageType( ResistanceType.Poison,   20 );
            SetDamageType( ResistanceType.Fire,     10 );
            SetDamageType( ResistanceType.Energy,   10 );

            SetResistance( ResistanceType.Physical, 50, 60 );
            SetResistance( ResistanceType.Fire,     40, 50 );
            SetResistance( ResistanceType.Cold,     30, 40 );
            SetResistance( ResistanceType.Poison,   50, 60 );
            SetResistance( ResistanceType.Energy,   50, 60 );

            // —— Skills ——
            SetSkill( SkillName.Wrestling, 110.0, 120.0 );
            SetSkill( SkillName.Tactics,   110.0, 120.0 );
            SetSkill( SkillName.Swords,    110.0, 120.0 );
            SetSkill( SkillName.MagicResist,100.0,110.0 );
            SetSkill( SkillName.Hiding,    120.0 );
            SetSkill( SkillName.Stealth,   120.0 );
            SetSkill( SkillName.Poisoning, 100.0,110.0 );
            SetSkill( SkillName.Anatomy,   100.0,110.0 );

            Fame           = 25000;
            Karma          = -25000;
            VirtualArmor   = 70;
            ControlSlots   = 5;

            // —— Gear & Loot ——            
            var jacket = new LeatherNinjaJacket { Hue = UniqueHue, Movable = false };
            AddItem( jacket );
            AddItem( new LeatherNinjaHood  { Hue = UniqueHue, Movable = false } );
            AddItem( new LeatherNinjaPants { Hue = UniqueHue, Movable = false } );
            AddItem( new LeatherNinjaMitts { Hue = UniqueHue, Movable = false } );
            AddItem( new NinjaTabi        { Hue = UniqueHue, Movable = false } );
            
            PackGold( 800, 1200 );
            PackItem( new Bone( Utility.RandomMinMax( 10, 20 ) ) );
            PackGem( 2 );

            // Schedule special moves
            var now = DateTime.UtcNow;
            m_NextShadowStep = now + TimeSpan.FromSeconds( Utility.RandomMinMax( 15, 25 ) );
            m_NextTrap       = now + TimeSpan.FromSeconds( Utility.RandomMinMax( 12, 20 ) );
            m_NextCurse      = now + TimeSpan.FromSeconds( Utility.RandomMinMax( 10, 15 ) );
            m_NextSwipe      = now + TimeSpan.FromSeconds( Utility.RandomMinMax( 8, 12 ) );
        }

        public override void OnThink()
        {
            base.OnThink();

            if ( !Alive || Map == null || Map == Map.Internal || Combatant == null )
                return;

            var now = DateTime.UtcNow;

            if ( now >= m_NextShadowStep && this.InRange( Combatant.Location, 10 ) )
            {
                ShadowStep();
                m_NextShadowStep = now + TimeSpan.FromSeconds( Utility.RandomMinMax( 20, 30 ) );
            }
            else if ( now >= m_NextTrap )
            {
                LayTrap();
                m_NextTrap = now + TimeSpan.FromSeconds( Utility.RandomMinMax( 15, 25 ) );
            }
            else if ( now >= m_NextCurse && this.InRange( Combatant.Location, 8 ) )
            {
                CurseOfTheSands();
                m_NextCurse = now + TimeSpan.FromSeconds( Utility.RandomMinMax( 18, 28 ) );
            }
            else if ( now >= m_NextSwipe && this.InRange( Combatant.Location, 3 ) )
            {
                SandSwipe();
                m_NextSwipe = now + TimeSpan.FromSeconds( Utility.RandomMinMax( 10, 18 ) );
            }
        }

        private void ShadowStep()
        {
            if ( Combatant is Mobile target && CanBeHarmful( target, false ) )
            {
                Say( "*The sands claim your path…*" );
                PlaySound( 0x55D );
                Effects.SendMovingParticles(
                    new Entity( Serial.Zero, this.Location, this.Map ),
                    new Entity( Serial.Zero, target.Location, this.Map ),
                    0x3818, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0
                );

                // Teleport right behind them
                Point3D behind = GetPointBehind( target, 1 );
                if ( this.Map.CanFit( behind, 16, false, false ) )
                    this.MoveToWorld( behind, this.Map );

                DoHarmful( target );
                int dmg = Utility.RandomMinMax( 50, 80 );
                AOS.Damage( target, this, dmg, 100, 0, 0, 0, 0 );
            }
        }

        private void LayTrap()
        {
            if ( Map == null ) return;

            Say( "*Feel the ancient ward…*" );
            PlaySound( 0x2D6 );
            var loc = this.Location;
            var tile = new LandmineTile { Hue = UniqueHue };
            tile.MoveToWorld( loc, this.Map );
        }

        private void CurseOfTheSands()
        {
            if ( Combatant is Mobile target && CanBeHarmful( target, false ) )
            {
                Say( "*By the curse of the tomb…*" );
                PlaySound( 0x1F1 );
                target.FixedParticles( 0x376A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head );

                target.ApplyPoison( this, Poison.Lethal );
                DoHarmful( target );
            }
        }

        private void SandSwipe()
        {
            if ( Combatant is Mobile primary && CanBeHarmful( primary, false ) )
            {
                Say( "*Desert winds!*" );
                PlaySound( 0x227 );
                Effects.SendLocationParticles(
                    EffectItem.Create( this.Location, this.Map, EffectItem.DefaultDuration ),
                    0x3728, 5, 10, UniqueHue, 0, 5039, 0
                );

                // Cone: any hostile in 3‑tile arc behind you
                foreach ( Mobile m in this.GetMobilesInRange( 3 ) )
                {
                    if ( m == this || !CanBeHarmful( m, false ) )
                        continue;

                    int dirTo   = (int)this.GetDirectionTo( m );
                    int opposite= ((int)this.Direction + 4) & 7;
                    int low     = (opposite - 1 + 8) & 7;
                    int high    = (opposite + 1)     & 7;

                    bool inArc = (low <= high)
                              ? (dirTo >= low && dirTo <= high)
                              : (dirTo >= low || dirTo <= high);

                    if ( !inArc )
                        continue;

                    DoHarmful( m );
                    int dmg = Utility.RandomMinMax( 30, 50 );
                    AOS.Damage( m, this, dmg, 60, 20, 0, 0, 20 );

                    if ( m is Mobile mm )
                    {
                        int drain = Utility.RandomMinMax( 10, 20 );
                        mm.Stam = Math.Max( 0, mm.Stam - drain );
                        mm.SendMessage( 0x22, "You feel your vigor sapped by the swirling sands!" );
                    }
                }
            }
        }

        // ——— Helpers ———

        /// <summary>
        /// Returns the direction exactly opposite to the given one.
        /// </summary>
        private static Direction GetOppositeDirection( Direction d )
        {
            return (Direction)( ((int)d + 4) & 7 );
        }

        /// <summary>
        /// Computes the point <paramref name="dist"/> tiles directly behind the mobile,
        /// based on its current facing direction.
        /// </summary>
        private static Point3D GetPointBehind( Mobile m, int dist )
        {
            Direction dir = GetOppositeDirection( m.Direction );
            var loc = m.Location;
            int dx = 0, dy = 0;

            switch ( dir )
            {
                case Direction.North:       dy = -dist;               break;
                case Direction.Right:       dx = +dist; dy = -dist;    break;
                case Direction.East:        dx = +dist;               break;
                case Direction.Down:        dx = +dist; dy = +dist;    break;
                case Direction.South:       dy = +dist;               break;
                case Direction.Left:        dx = -dist; dy = +dist;    break;
                case Direction.West:        dx = -dist;               break;
                case Direction.Up:          dx = -dist; dy = -dist;    break;
            }

            return new Point3D( loc.X + dx, loc.Y + dy, loc.Z );
        }

        // ——— Standard Overrides ———

        public override void OnDeath( Container c )
        {
            base.OnDeath( c );
			
			if (Map == null)
				return;			
            c.DropItem( new BonePile() );
            if ( Utility.RandomDouble() < 0.10 )
                c.DropItem( new PowerScroll( SkillName.Stealth, 105 ) );
        }

        public override void GenerateLoot()
        {
            AddLoot( LootPack.FilthyRich );
            AddLoot( LootPack.Gems, 2 );
            if ( Utility.RandomDouble() < 0.02 )
                PackItem( new Threadpiercer() );
        }

        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 140.0;
        public override double DispelFocus     => 70.0;

        public TombRobber( Serial serial ) : base( serial ) { }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int)0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();

            // re‑init timers
            var now = DateTime.UtcNow;
            m_NextShadowStep = now + TimeSpan.FromSeconds( Utility.RandomMinMax( 15, 25 ) );
            m_NextTrap       = now + TimeSpan.FromSeconds( Utility.RandomMinMax( 12, 20 ) );
            m_NextCurse      = now + TimeSpan.FromSeconds( Utility.RandomMinMax( 10, 15 ) );
            m_NextSwipe      = now + TimeSpan.FromSeconds( Utility.RandomMinMax( 8, 12 ) );
        }
    }
}
