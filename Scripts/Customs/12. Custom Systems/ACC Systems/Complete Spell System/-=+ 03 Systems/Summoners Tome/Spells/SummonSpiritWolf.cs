using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.SpiritSpeakMagic
{
	public class SummonSpiritWolf : SpiritSpeakSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
		                                                "Summon Spirit Wolf", "Spirits Come!",
		                                                //SpellCircle.Third,
		                                                266,
		                                                9040
		                                               );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

		public override double CastDelay{ get{ return 0.1; } }
		public override double RequiredSkill{ get{ return 20.0; } }
		public override int RequiredMana{ get{ return 10; } }

		public SummonSpiritWolf( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		private static Type[] m_Types = new Type[]
		{
			typeof( SummonedSpiritWolf ),
			typeof( SummonedSpiritWolf ),
			typeof( SummonedSpiritWolf ),
			typeof( SummonedSpiritWolf )

		};

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				try
				{
					Type beasttype = ( m_Types[Utility.Random( m_Types.Length )] );

					BaseCreature creaturea = (BaseCreature)Activator.CreateInstance( beasttype );
					BaseCreature creatureb = (BaseCreature)Activator.CreateInstance( beasttype );
					BaseCreature creaturec = (BaseCreature)Activator.CreateInstance( beasttype );
					BaseCreature creatured = (BaseCreature)Activator.CreateInstance( beasttype );


					SpellHelper.Summon( creaturea, Caster, 0x215, TimeSpan.FromSeconds( 4.0 * Caster.Skills[CastSkill].Value ), false, false );
					SpellHelper.Summon( creatureb, Caster, 0x215, TimeSpan.FromSeconds( 4.0 * Caster.Skills[CastSkill].Value ), false, false );

					Double morebeast = 0 ;

					morebeast = Utility.Random( 10 ) + ( Caster.Skills[CastSkill].Value * 0.1 );


					if ( morebeast > 11 )
					{
						SpellHelper.Summon( creaturec, Caster, 0x215, TimeSpan.FromSeconds( 4.0 * Caster.Skills[CastSkill].Value ), false, false );
					}

					if ( morebeast > 18 )
					{
						SpellHelper.Summon( creatured, Caster, 0x215, TimeSpan.FromSeconds( 4.0 * Caster.Skills[CastSkill].Value ), false, false );
					}
				}
				catch
				{
				}
			}

			FinishSequence();
		}
	}
}
