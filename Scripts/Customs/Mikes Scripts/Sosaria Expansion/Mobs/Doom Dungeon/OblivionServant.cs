using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("an oblivion servant corpse")]
    public class OblivionServant : BaseCreature
    {
        // Cooldowns for special attacks
        private DateTime m_NextSiphonTime;
        private DateTime m_NextChainsTime;
        private DateTime m_NextVoidTime;
        private Point3D m_LastLocation;

        // A deep, void‑touched purple
        private const int UniqueHue = 1175;

        [Constructable]
        public OblivionServant()
            : base( AIType.AI_Mage, FightMode.Closest, 12, 1, 0.1, 0.2 )
        {
            SpeechHue = Utility.RandomDyedHue();
            Hue       = UniqueHue;
            Title     = "Oblivion Servant";

            // — Body & Appearance (as ForgottenServant) —
            if ( this.Female = Utility.RandomBool() )
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                AddItem( new Skirt(UniqueHue) );
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                AddItem( new ShortPants(UniqueHue) );
            }

            // — Stats & Resistances —
            SetStr(250, 300);
            SetDex(150, 200);
            SetInt(450, 550);

            SetHits(1200, 1400);
            SetStam(150, 200);
            SetMana(800, 900);

            SetDamage(20, 30);

            SetDamageType( ResistanceType.Physical, 10 );
            SetDamageType( ResistanceType.Energy, 90 );

            SetResistance( ResistanceType.Physical, 50, 60 );
            SetResistance( ResistanceType.Fire,     40, 50 );
            SetResistance( ResistanceType.Cold,     40, 50 );
            SetResistance( ResistanceType.Poison,   30, 40 );
            SetResistance( ResistanceType.Energy,   80, 90 );

            SetSkill( SkillName.EvalInt,     120.1, 135.0 );
            SetSkill( SkillName.Magery,      120.1, 135.0 );
            SetSkill( SkillName.MagicResist, 120.2, 135.0 );
            SetSkill( SkillName.Meditation,  110.0, 120.0 );
            SetSkill( SkillName.Tactics,     95.1, 105.0 );
            SetSkill( SkillName.Wrestling,   95.1, 105.0 );

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 90;
            ControlSlots = 5;

            // Set up ability timers
            m_NextSiphonTime = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax(12, 16) );
            m_NextChainsTime = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax(18, 24) );
            m_NextVoidTime   = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax(25, 30) );
            m_LastLocation   = this.Location;

            // Starter loot
            PackItem( new BlackPearl( Utility.RandomMinMax(15, 20) ) );
            PackItem( new Nightshade( Utility.RandomMinMax(15, 20) ) );
            PackItem( new SulfurousAsh( Utility.RandomMinMax(15, 20) ) );
        }

        // — Leave a bit of void residue as it moves —
        public override void OnMovement( Mobile m, Point3D oldLocation )
        {
            if ( this.Alive && m != this && m.Map == Map && m.InRange( this.Location, 2 ) && m is Mobile targetMobile && CanBeHarmful( targetMobile, false ) )
            {
                // Spawn a short‑lived PoisonTile in its wake
                if ( this.Map.CanFit( oldLocation.X, oldLocation.Y, oldLocation.Z, 16, false, false ) )
                {
                    var tile = new PoisonTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld( oldLocation, this.Map );
                }
            }

            base.OnMovement( m, oldLocation );
        }

        public override void OnThink()
        {
            base.OnThink();

            if ( Combatant == null || Map == null || Map == Map.Internal || !Alive )
                return;

            var now = DateTime.UtcNow;

            // Priority: Soul Siphon → Oblivion Chains → Void Surge
            if ( now >= m_NextSiphonTime && InRange( Combatant.Location, 10 ) )
            {
                SoulSiphon();
                m_NextSiphonTime = now + TimeSpan.FromSeconds( Utility.RandomMinMax(14, 18) );
            }
            else if ( now >= m_NextChainsTime && InRange( Combatant.Location, 8 ) )
            {
                OblivionChains();
                m_NextChainsTime = now + TimeSpan.FromSeconds( Utility.RandomMinMax(20, 26) );
            }
            else if ( now >= m_NextVoidTime && InRange( Combatant.Location, 12 ) )
            {
                VoidSurge();
                m_NextVoidTime = now + TimeSpan.FromSeconds( Utility.RandomMinMax(30, 36) );
            }
        }

        // — Single‑target life‑and‑mana drain, heals self —
        private void SoulSiphon()
        {
            if ( Combatant is Mobile target && CanBeHarmful( target, false ) )
            {
                Say( "*Your essence is mine!*" );
                PlaySound( 0x1F9 );
                FixedParticles( 0x375A, 10, 15, 5019, UniqueHue, 0, EffectLayer.Waist );

                DoHarmful( target );

                int drainHP   = Utility.RandomMinMax( 40, 60 );
                int drainMana = Utility.RandomMinMax( 30, 50 );

                // Damage & drain
                AOS.Damage( target, this, drainHP, 0, 0, 0, 0, 100 );
                if ( target.Mana >= drainMana )
                    target.Mana -= drainMana;

                // Heal self
                this.Hits += drainHP / 2;
                this.Mana += drainMana / 2;
            }
        }

        // — Chains root all foes in a 4‑tile radius, then damage —
        private void OblivionChains()
        {
            Say( "*Bound by void!*" );
            PlaySound( 0x20C );
            FixedParticles( 0x3790, 8, 20, 5005, UniqueHue, 0, EffectLayer.Head );

            var targets = new List<Mobile>();
            foreach ( Mobile m in Map.GetMobilesInRange( this.Location, 4 ) )
            {
                if ( m != this && m.Alive && CanBeHarmful( m, false ) && SpellHelper.ValidIndirectTarget( this, m ) )
                    targets.Add( m );
            }

            foreach ( var tgt in targets )
            {
                DoHarmful( tgt );
                // Root for 5 seconds
                tgt.Freeze( TimeSpan.FromSeconds(5) );
                // Damage over time
                Timer.DelayCall( TimeSpan.FromSeconds(1.0), 
                    delegate { if ( CanBeHarmful( tgt, false ) ) AOS.Damage( tgt, this, Utility.RandomMinMax(20, 30), 0, 0, 0, 0, 100 ); } );
            }
        }

        // — Spawn a ring of hazardous tiles around the target —
        private void VoidSurge()
        {
            if ( !(Combatant is Mobile target) || !CanBeHarmful( target, false ) )
                return;

            Say( "*Feel the void's embrace!*" );
            PlaySound( 0x22F );

            var center = target.Location;
            for ( int i = 0; i < 6; i++ )
            {
                double angle = (Math.PI * 2 / 6) * i;
                int xOff = (int)Math.Round( Math.Cos(angle) * 3 );
                int yOff = (int)Math.Round( Math.Sin(angle) * 3 );
                var loc = new Point3D( center.X + xOff, center.Y + yOff, center.Z );

                // Adjust Z if needed
                if ( !Map.CanFit( loc.X, loc.Y, loc.Z, 16, false, false ) )
                    loc.Z = Map.GetAverageZ( loc.X, loc.Y );

                var tile = new QuicksandTile(); 
                tile.Hue = UniqueHue;
                tile.MoveToWorld( loc, Map );
            }
        }

        // — Explosive death effect —
        public override void OnDeath( Container c )
        {
            if ( Map != null )
            {
                Say( "*Eternal void!*" );
                PlaySound( 0x211 );
                Effects.SendLocationParticles( EffectItem.Create( this.Location, Map, EffectItem.DefaultDuration ),
                                              0x3709, 10, 60, UniqueHue, 0, 5052, 0 );

                // Spawn a handful of PoisonTiles and VortexTiles
                for ( int i = 0; i < Utility.RandomMinMax(4,7); i++ )
                {
                    int xOff = Utility.RandomMinMax(-3,3);
                    int yOff = Utility.RandomMinMax(-3,3);
                    var loc = new Point3D( X + xOff, Y + yOff, Z );

                    if ( !Map.CanFit( loc.X, loc.Y, loc.Z, 16, false, false ) )
                        loc.Z = Map.GetAverageZ( loc.X, loc.Y );

                    var poison = new PoisonTile();
                    poison.Hue = UniqueHue;
                    poison.MoveToWorld( loc, Map );

                    var vortex = new VortexTile();
                    vortex.Hue = UniqueHue;
                    vortex.MoveToWorld( loc, Map );
                }
            }

            base.OnDeath(c);
        }

        // — Loot & Rewards —
        public override void GenerateLoot()
        {
            AddLoot( LootPack.UltraRich, 2 );
            AddLoot( LootPack.HighScrolls, Utility.RandomMinMax(1,2) );
            AddLoot( LootPack.Gems, Utility.RandomMinMax(8,12) );

            if ( Utility.RandomDouble() < 0.03 ) // 3% chance
                PackItem( new PsychicNeedle() );  // your custom unique artifact
        }

        public override bool BleedImmune    => true;
        public override int  TreasureMapLevel => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus      => 75.0;

        // — Serialization Boilerplate —
        public OblivionServant( Serial serial ) : base(serial) { }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset cooldowns on reload
            m_NextSiphonTime = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax(12, 16) );
            m_NextChainsTime = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax(18, 24) );
            m_NextVoidTime   = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax(25, 30) );
            m_LastLocation   = this.Location;
        }
    }
}
